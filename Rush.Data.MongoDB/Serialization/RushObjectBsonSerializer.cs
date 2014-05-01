namespace Rush.Data.Serialization
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Runtime.CompilerServices;
    using Microsoft.CSharp.RuntimeBinder;
    using MongoDB.Bson.IO;
    using MongoDB.Bson.Serialization;
    using MongoDB.Bson.Serialization.IdGenerators;
    using MongoDB.Bson.Serialization.Options;
    using MongoDB.Bson.Serialization.Serializers;

    public sealed class RushObjectBsonSerializer : DynamicDocumentBaseSerializer<RushObject>, IBsonIdProvider, IBsonDocumentSerializer
    {
        private static readonly RushObjectBsonSerializer Instance = new RushObjectBsonSerializer(); 

        private static readonly IBsonSerializer<object> _objectSerializer = BsonSerializer.LookupSerializer<object>();
        private readonly IBsonSerializer<List<object>> _listSerializer;

        public RushObjectBsonSerializer()
        {
            _listSerializer = BsonSerializer.LookupSerializer<List<object>>();
        }

        #region DynamicDocumentBaseSerializer

        protected override void ConfigureDeserializationContext(BsonDeserializationContext.Builder builder)
        {
            builder.DynamicDocumentSerializer = this;
            builder.DynamicArraySerializer = _listSerializer;
        }

        protected override void ConfigureSerializationContext(BsonSerializationContext.Builder builder)
        {
            builder.IsDynamicType = t => (t == typeof(RushObject)) || (t == typeof(List<object>));
        }

        protected override RushObject CreateDocument()
        {
            return new RushObject();
        }

        protected override void SetValueForMember(RushObject document, string memberName, object value)
        {
            document[memberName] = value;
        }

        protected override bool TryGetValueForMember(RushObject document, string memberName, out object memberValue)
        {
            return document.TryGetValue(memberName, out memberValue);
        }

        public override void Serialize(BsonSerializationContext context, RushObject value)
        {
            var bsonWriter = context.Writer;
            bsonWriter.WriteStartDocument();

            var properties = value.GetPropertyValues();
            foreach (var property in properties)
            {
                object memberValue = property.Value;
                bsonWriter.WriteName(property.Key);
                context.SerializeWithChildContext<object>(_objectSerializer, memberValue, ConfigureSerializationContext);
            }

            bsonWriter.WriteEndDocument();
        } 

        #endregion

        #region IBsonIdProvider

        public bool GetDocumentId(object document, out object id, out Type idNominalType, out IIdGenerator idGenerator)
        {
            var rushObject = document as RushObject;
            id = rushObject["ObjectId"] ?? rushObject["_id"];
            idNominalType = typeof(string);
            idGenerator = StringObjectIdGenerator.Instance;
            return true;
        }

        public void SetDocumentId(object document, object id)
        {
            var rushObject = document as RushObject;
            rushObject["ObjectId"] = rushObject["_id"] = id;
        } 

        #endregion

        #region IBsonDocumentSerializer

        public BsonSerializationInfo GetMemberSerializationInfo(string memberName)
        {
            return new BsonSerializationInfo(memberName, new ObjectSerializer(), typeof(object));
        }

        void IBsonSerializer.Serialize(BsonSerializationContext context, object value)
        {
            this.Serialize(context, value as RushObject);
        }

        object IBsonSerializer.Deserialize(BsonDeserializationContext context)
        {
            return this.Deserialize(context);
        }

        #endregion
    }

    public sealed class RushObjectSerializationProvider : IBsonSerializationProvider
    {
        public IBsonSerializer GetSerializer(Type type)
        {
            if (typeof(RushObject).IsAssignableFrom(type))
                return new RushObjectBsonSerializer();
            return null;
        }
    }
}