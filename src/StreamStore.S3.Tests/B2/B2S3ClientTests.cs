using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamStore.S3.Tests.B2
{
    public  class B2S3ClientTests : S3ClientTests
    {
        public B2S3ClientTests() : base(B2TestsSuite.CreateFactory())
        {
        }
    
    }
}
