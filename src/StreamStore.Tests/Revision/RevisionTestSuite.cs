﻿using StreamStore.Testing.Framework;

namespace StreamStore.Tests.RevisionObject
{
    public class RevisionTestSuite: TestSuiteBase
    {

        public static Revision CreateRevision(int revision = 0)
        {
            return new Revision(revision);
        }
    }
}
