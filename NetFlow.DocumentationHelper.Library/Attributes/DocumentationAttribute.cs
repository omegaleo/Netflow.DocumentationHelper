﻿using System;

namespace NetFlow.DocumentationHelper.Library.Attributes
{
    [System.AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class DocumentationAttribute : Attribute
    {
        public string Title;
        public string Description;
        public string[] Args;

        public DocumentationAttribute(string title, string description, string[] args = default)
        {
            Title = title;
            Description = description;
            Args = args;
        }
    }
}