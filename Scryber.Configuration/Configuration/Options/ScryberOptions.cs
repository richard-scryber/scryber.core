using System;
namespace Scryber.Configuration.Options
{
    public class ScryberOptions
    {
        public const string ScryberSection = "Scryber";

        public NamespaceOptions[] Namespaces { get; set; }

        public BindingOptions[] Bindings { get; set; }

        public FontOptions Fonts { get; set; }

        public RenderOptions Render { get; set; }

        public ScryberOptions()
        {
        }
    }
}
