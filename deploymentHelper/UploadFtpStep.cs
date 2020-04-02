using System;
using System.Xml;

namespace DeploymentHelper
{
    namespace Steps
    {
        public class UploadFtpStep : ExecutableDeploymentStep
        {
            public UploadFtpStep(Step parent, XmlNode node) : base(parent, node)
            {
            }

            public override bool ExecuteStep()
            {
                throw new NotImplementedException();
            }
        }
    }
}