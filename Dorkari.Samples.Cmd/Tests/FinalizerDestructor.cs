using System;

namespace Dorkari.Samples.Cmd.Tests
{
    class FinalizerDestructor
    {
    }

    class DerivedClass : FinalizerDestructor // the inheritance doesn't matter though
    {
        //protected override void Finalize()
        //{ } ^ NOT allowed in C#, use a destructor instead

        //C# destructor == .NET finalizer
        //used to release unmanaged resources*, VERY rare actually *e.g. Window handles, external pointer etc.
        ~DerivedClass() //no access modifier
        {
            //Clean all unmanaged resources - use ONLY IF REQUIRED
            //It slows down performance as GC will put this in Finalize queue and process again

            //This will be called by GC undeterministically - i.e.
            //not at any specific time, any order or specific therad
            //base.Finalize(); this is implicitly called by GC
            //finalizer/Destructor is called recursively thorugh the inheritance chain, bottom-to-top
        }
    }

    //With IDisposable, Dispose() can be called deterministically
    //either directly or by using a using statement using(var v = new MyDisposable()) { ... }
    //Apart from unmanaged resources, it should also release managed resources like
    //file handles or database connections, so that they are released early
    class MyDisposable : IDisposable //a recommended pattern
    {
        // Track whether Dispose has been called.
        private bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposingViaCode)
        {
            // Check to see if Dispose has already been called.
            if (!disposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposingViaCode)
                {
                    // Dispose managed resources.
                }

                // Call the appropriate methods to clean up
                // unmanaged resources here.
                // If disposing is false,
                // only the following code is executed.
                //CloseHandle(handle);
                //handle = IntPtr.Zero;

                // Note disposing has been done.
                disposed = true;
            }
        }

        //call GC.SuppressFinalize(this) ONLY if it has a destructor/finalizer
        ~MyDisposable()
        {
            Dispose(false);
        }
    }
}
