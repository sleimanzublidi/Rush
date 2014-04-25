namespace Rush
{
    using System;

    public interface IRushController
    {
        void Get();
        void Post();
        void Put();
        void Delete();
    }

    public abstract class RushController : IRushController
    {
        public RushController(RushContext context)
        {
            this.Context = context;
        }

        protected RushContext Context { get; private set; }

        public virtual void Get()
        {
            throw new NotSupportedException();
        }

        public virtual void Post()
        {
            throw new NotSupportedException();
        }

        public virtual void Put()
        {
            throw new NotSupportedException();
        }

        public virtual void Delete()
        {
            throw new NotSupportedException();
        }
    }
}
