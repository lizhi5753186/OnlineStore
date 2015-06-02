using System;

namespace OnlineStore.Infrastructure
{
    public abstract class DisposableObject :IDisposable
    {
        ~DisposableObject()
        {
            this.Dispose(false);
        }
      
        protected abstract void Dispose(bool disposing);
        
        protected void ExplicitDispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        #region IDisposable Members
        
        public void Dispose()
        {
            this.ExplicitDispose();
        }
        #endregion
    }
}