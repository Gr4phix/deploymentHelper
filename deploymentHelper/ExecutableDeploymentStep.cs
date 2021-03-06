﻿using System;

namespace DeploymentHelper
{
    namespace Steps
    {
        public abstract class ExecutableDeploymentStep
        {
            protected Step parent;

            protected ExecutableDeploymentStep(Step parent)
            {
                this.parent = parent;

                Console.WriteLine($"\tAdded new step of type: {parent.Type}");
            }
        }
    }
}