using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Synchronization;
using Microsoft.Synchronization.Files;
using System.IO;

namespace Neutrine.YouSync.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class UnitTest1
    {
        public UnitTest1()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestMethod1()
        {
            String idFileName = ".yousync";

            String sourcePath = @"C:\temp\app\app";
            String destPath = @"C:\temp\sync";
            Guid sourceId = GetReplicaId(Path.Combine(sourcePath, idFileName));
            Guid destId = GetReplicaId(Path.Combine(destPath, idFileName));
            
            FileSyncProvider sourceProvider = new FileSyncProvider(sourceId, sourcePath);
            FileSyncProvider destProvider = new FileSyncProvider(destId, destPath);

            sourceProvider.

            SyncOrchestrator sync = new SyncOrchestrator();
            sync.LocalProvider = sourceProvider;
            sync.RemoteProvider = destProvider;
            sync.Direction = SyncDirectionOrder.UploadAndDownload;
            sync.Synchronize();
        }


        public Guid GetReplicaId(string idFilePath)
        {
            Guid replicaId = Guid.Empty;

            if (File.Exists(idFilePath))
            {
                using (StreamReader sr = File.OpenText(idFilePath))
                {
                    string strGuid = sr.ReadLine();
                    if (!string.IsNullOrEmpty(strGuid))
                        replicaId = new Guid(strGuid);
                }
            }

            if (replicaId == Guid.Empty)
            {
                using (FileStream idFile = File.Open(
                            idFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    using (StreamWriter sw = new StreamWriter(idFile))
                    {
                        replicaId = Guid.NewGuid();
                        sw.WriteLine(replicaId.ToString("D"));
                    }
                }
            }

            return replicaId;
        }
    }
}
