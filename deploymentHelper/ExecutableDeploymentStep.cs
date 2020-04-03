using System;
using System.Xml;

namespace DeploymentHelper
{
    namespace Steps
    {
        public abstract class ExecutableDeploymentStep
        {
            protected Step parent;

            protected ExecutableDeploymentStep(Step parent, XmlNode node)
            {
                this.parent = parent;

                Console.WriteLine($"\tAdded new step of type: {parent.Type}");
            }

            public abstract bool ExecuteStep();
        }
    }
}