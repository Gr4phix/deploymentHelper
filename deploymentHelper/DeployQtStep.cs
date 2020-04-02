using System;
using System.Xml;

namespace DeploymentHelper
{
    namespace Steps
    {
        public class DeployQtStep : ExecutableDeploymentStep
        {
            public DeployQtStep(Step parent, XmlNode node) : base(parent, node)
            {
            }

            public override bool ExecuteStep()
            {
                throw new NotImplementedException();
            }
        }
    }
}