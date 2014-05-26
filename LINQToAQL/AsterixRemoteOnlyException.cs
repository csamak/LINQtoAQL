using System;

namespace LINQToAQL
{
    internal class AsterixRemoteOnlyException : Exception
    {
        public override string Message
        {
            get { return "Method call can only be evaluated remotely by AsterixDB."; }
        }
    }
}