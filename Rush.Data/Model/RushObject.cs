namespace Rush
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Dynamic;
    using System.Linq;
    using System.Reflection;

    public class RushObject : DynamicObject, INotifyPropertyChanged
    {
        internal object instance;
        internal Type instanceType;
        private readonly Dictionary<string, object> properties = new Dictionary<string, object>();

        public RushObject()
        {
            Initialize(this, null);
        }

        public RushObject(string className)
        {
            if (String.IsNullOrWhiteSpace(className))
                throw new ArgumentException("className was not defined.");
            Initialize(this, className);
        }

        public RushObject(object instance)
        {
            Initialize(instance, null);
        }

        protected virtual void Initialize(object instance, string className)
        {
            this.instance = instance;
            this.instanceType = instance.GetType();
            this.ClassName = String.IsNullOrWhiteSpace(className) ? instanceType.Name : className;
        }

        #region Properties

        public string ClassName { get; set; }        
        public string ObjectId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        #endregion

        #region Dynamic Properties

        public object this[string propertyName]
        {
            get { return GetValue(propertyName); }
            set { SetValue(propertyName, value); }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        private void SetValue(string propertyName, object value)
        {
            TrySetMember(new SetMemberValueBinder(propertyName), value);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        sealed public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (instance != null)
            {
                try
                {
                    bool result = SetProperty(instance, binder.Name, value);
                    if (result) return true;
                }
                catch { }
            }

            properties[binder.Name] = value;
            NotifyPropertyChanged(binder.Name);
            return true;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        private object GetValue(string propertyName)
        {
            object value = null;
            TryGetMember(new GetMemberValueBinder(propertyName), out value);
            return value;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool TryGetValue(string propertyName, out object value)
        {
            return TryGetMember(new GetMemberValueBinder(propertyName), out value);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        sealed public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (properties.Keys.Contains(binder.Name, StringComparer.OrdinalIgnoreCase))
            {
                result = properties[binder.Name];
                return true;
            }

            if (instance != null)
            {
                try
                {
                    return GetProperty(instance, binder.Name, out result);
                }
                catch { }
            }

            result = null;
            return false;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected bool GetProperty(object instance, string name, out object result)
        {
            if (instance == null) instance = this;

            var members = instanceType.GetMember(name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (members != null && members.Length > 0)
            {
                var memberInfo = members[0];
                if (memberInfo is PropertyInfo)
                {
                    result = ((PropertyInfo)memberInfo).GetValue(instance, null);
                    return true;
                }
                else if (memberInfo is FieldInfo)
                {
                    result = ((FieldInfo)memberInfo).GetValue(instance);
                    return true;
                }   
            }

            result = null;
            return false;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected bool SetProperty(object instance, string name, object value)
        {
            if (instance == null) instance = this;

            var members = instanceType.GetMember(name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (members != null && members.Length > 0)
            {
                var memberInfo = members[0];
                if (memberInfo is PropertyInfo)
                {
                    ((PropertyInfo)memberInfo).SetValue(instance, value, null);
                    return true;
                }
                else if (memberInfo is FieldInfo)
                {
                    ((FieldInfo)memberInfo).SetValue(instance, value);
                    return true;
                }
            }
            return false;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        sealed public override IEnumerable<string> GetDynamicMemberNames()
        {
            return properties.Keys;
        }

        PropertyInfo[] instancePropertyInfo;
        PropertyInfo[] InstancePropertyInfo
        {
            get
            {
                if (instancePropertyInfo == null && instance != null)
                {
                    instancePropertyInfo = instance.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                }
                return instancePropertyInfo;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public IEnumerable<KeyValuePair<string, object>> GetPropertyValues()
        {
            if (instance != null)
            {
                foreach (var property in this.InstancePropertyInfo)
                {
                    if (property.GetIndexParameters().Length == 0)
                        yield return new KeyValuePair<string, object>(property.Name, property.GetValue(instance, null));
                }   
            }

            foreach (var key in properties.Keys)
                yield return new KeyValuePair<string, object>(key, properties[key]);
        }

        #endregion

        #region Methods

        public T Get<T>(string propertyName)
        {
            return (T)this[propertyName];
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region Binders

        class SetMemberValueBinder : SetMemberBinder
        {
            public SetMemberValueBinder(string propertyName)
                : base(propertyName, true)
            { }

            public override DynamicMetaObject FallbackSetMember(DynamicMetaObject target, DynamicMetaObject value, DynamicMetaObject errorSuggestion)
            {
                return errorSuggestion;
            }
        }

        class GetMemberValueBinder : GetMemberBinder
        {
            public GetMemberValueBinder(string propertyName)
                : base(propertyName, true)
            { }

            public override DynamicMetaObject FallbackGetMember(DynamicMetaObject target, DynamicMetaObject errorSuggestion)
            {
                return errorSuggestion;
            }
        }
        #endregion

        #region DynamicObject Overrides

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override DynamicMetaObject GetMetaObject(System.Linq.Expressions.Expression parameter)
        {
            return base.GetMetaObject(parameter);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool TryBinaryOperation(BinaryOperationBinder binder, object arg, out object result)
        {
            return base.TryBinaryOperation(binder, arg, out result);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            return base.TryConvert(binder, out result);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool TryCreateInstance(CreateInstanceBinder binder, object[] args, out object result)
        {
            return base.TryCreateInstance(binder, args, out result);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool TryDeleteIndex(DeleteIndexBinder binder, object[] indexes)
        {
            return base.TryDeleteIndex(binder, indexes);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool TryDeleteMember(DeleteMemberBinder binder)
        {
            return base.TryDeleteMember(binder);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            return base.TryGetIndex(binder, indexes, out result);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool TryInvoke(InvokeBinder binder, object[] args, out object result)
        {
            return base.TryInvoke(binder, args, out result);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            return base.TryInvokeMember(binder, args, out result);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
        {
            return base.TrySetIndex(binder, indexes, value);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool TryUnaryOperation(UnaryOperationBinder binder, out object result)
        {
            return base.TryUnaryOperation(binder, out result);
        }

        #endregion
    }
}
