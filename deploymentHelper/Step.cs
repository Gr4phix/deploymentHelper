﻿using DeploymentHelper.Steps;
using System;
using System.Xml;

namespace DeploymentHelper
{
    public class Step
    {
        public EStepType Type { get; }

        private readonly ExecutableDeploymentStep childStep;

        public Step(XmlNode stepNode)
        {
            Type = stepNode.Name switch
            {
                "deploy-qt" => EStepType.DEPLOY_QT,
                "build-doc" => EStepType.BUILD_DOC,
                "upload-ftp" => EStepType.UPLOAD_FTP,
                "msbuild" => EStepType.MSBUILD,
                "clean" => EStepType.CLEAN,
                "latexcompile" => EStepType.LATEX_COMPILE,
                _ => EStepType.NONE,
            };

            childStep = Type switch
            {
                EStepType.DEPLOY_QT => new DeployQtStep(this, stepNode),
                EStepType.BUILD_DOC => new BuildDocStep(this, stepNode),
                EStepType.UPLOAD_FTP => new UploadFtpStep(this, stepNode),
                EStepType.MSBUILD => new MsBuildStep(this, stepNode),
                EStepType.CLEAN => new CleanStep(this, stepNode),
                EStepType.LATEX_COMPILE => new LatexCompileStep(this, stepNode),
                EStepType.NONE => null,
                _ => null,
            };
        }

        public bool ExecuteStep()
        {
            switch (Type)
            {
                case EStepType.DEPLOY_QT:
                case EStepType.BUILD_DOC:
                case EStepType.MSBUILD:
                    try
                    {
                        CmdExecuteHelper.ExecuteCommand(childStep.ToString());
                    }
                    catch (Exception)
                    {
                        return false;
                    }

                    return true;

                case EStepType.UPLOAD_FTP:
                    return ((UploadFtpStep)childStep).ExecuteStep();

                case EStepType.LATEX_COMPILE:
                    return ((LatexCompileStep)childStep).ExecuteStep();

                case EStepType.CLEAN:
                    return ((CleanStep)childStep).ExecuteStep();
            }

            return false;
        }
    }
}