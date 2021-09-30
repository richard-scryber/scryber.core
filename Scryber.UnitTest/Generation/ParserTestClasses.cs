using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scryber.Core.UnitTests.Generation.Fakes
{
    //
    // These classes are used by the Parser tests to validate the attribute
    // reflection and xml parser.
    //


    /// <summary>
    /// A top level root component that can be reflected and inspected
    /// </summary>
    /// 
    [PDFParsableComponent("Root1")]
    [PDFRequiredFramework("0.8.0.0")]
    public class ParserRootOne : IComponent
    {
        /// <summary>
        /// Simple read write name attribute
        /// </summary>
        [PDFAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// An inner complex component
        /// </summary>
        [PDFElement("Complex-Element")]
        public ParserInnerComplex Complex { get; set; }

        /// <summary>
        /// A collection of items within a surrounding element
        /// </summary>
        [PDFArray(typeof(ParserInnerComplex))]
        [PDFElement("Collection-One")]
        public ParserCollection CollectionOne { get; set; }

        /// <summary>
        /// A collection of items that are the default
        /// </summary>
        [PDFArray(typeof(ParserInnerBase))]
        [PDFElement("")]
        public ParserCollection DefaultCollection { get; set; }


        #region IPDFComponent implementation for parsing - don't use

        public event PDFInitializedEventHandler Initialized;

        void IComponent.Init(PDFInitContext context)
        {
            if (null != this.Initialized)
                this.Initialized(this, new PDFInitEventArgs(context));
        }


        public event PDFLoadedEventHandler Loaded;

        public void Load(PDFLoadContext context)
        {
            if (null != this.Loaded)
                this.Loaded(this, new PDFLoadEventArgs(context));
        }

        string IComponent.ElementName
        {
            get;
            set;
        }

        string IComponent.ID
        {
            get
            {
                return string.Empty;
            }
            set
            {
                
            }
        }

        IDocument IComponent.Document
        {
            get { return null; }
        }

        IComponent IComponent.Parent
        {
            get
            {
                return null;
            }
            set
            {
                
            }
        }

        string IComponent.MapPath(string source)
        {
            return source;
        }

        ObjectType ITypedObject.Type
        {
            get { return (ObjectType)"naob"; }
        }

        void IDisposable.Dispose()
        {
            
        }

        #endregion
    }

    public abstract class ParserInnerBase
    {
        /// <summary>
        /// Integer value
        /// </summary>
        [PDFAttribute("index")]
        public int Index { get; set; }

        /// <summary>
        /// No Attribute - no inclusion
        /// </summary>
        public int AnotherIndex { get; set; }

        /// <summary>
        /// string value that can be overriden
        /// </summary>
        [PDFAttribute("inherited")]
        public virtual string Inheritable { get; set; }

    }

    /// <summary>
    /// Concrete implementation of the abstract base class
    /// </summary>
    [PDFParsableComponent("Inner")]
    [PDFRemoteParsableComponent("Inner-Ref")]
    public class ParserInnerComplex : ParserInnerBase, IComponent
    {
        /// <summary>
        /// Overrides the base implementation
        /// </summary>
        [PDFAttribute("concrete", ParserDefinitionFactoryTests.NamespaceAssembly)]
        public override string Inheritable
        {
            get
            {
                return base.Inheritable;
            }
            set
            {
                base.Inheritable = value;
            }
        }

        /// <summary>
        /// Double value
        /// </summary>
        [PDFAttribute("size")]
        public double Size { get; set; }

        [PDFAttribute("date")]
        public DateTime Date { get; set; }

        /// <summary>
        /// Double value that cannot be written to.
        /// </summary>
        [PDFAttribute("ro-size")]
        public double ReadOnlySize
        {
            get { return Size; }
        }


        #region IPDFComponent implementation for parsing - Needed for remote reference

        public event PDFInitializedEventHandler Initialized;

        void IComponent.Init(PDFInitContext context)
        {
            if (null != this.Initialized)
                this.Initialized(this, new PDFInitEventArgs(context));
        }

        
        public event PDFLoadedEventHandler Loaded;

        public void Load(PDFLoadContext context)
        {
            if(null != this.Loaded)
                this.Loaded(this, new PDFLoadEventArgs(context));
        }

        string IComponent.ElementName
        {
            get;
            set;
        }

        string IComponent.ID
        {
            get
            {
                return string.Empty;
            }
            set
            {

            }
        }

        IDocument IComponent.Document
        {
            get { return null; }
        }

        IComponent IComponent.Parent
        {
            get
            {
                return null;
            }
            set
            {

            }
        }

        string IComponent.MapPath(string source)
        {
            return source;
        }

        ObjectType ITypedObject.Type
        {
            get { return (ObjectType)"naob"; }
        }

        void IDisposable.Dispose()
        {

        }

        #endregion
    }


    public class ParserCollection : List<ParserInnerBase>
    {
    }


    [PDFParsableComponent("Invalid-Inherits")]
    public class InvalidAttributeInherits
    {
        /// <summary>
        /// Will throw execption as inherits is a reserved attribute name
        /// </summary>
        [PDFAttribute("inherits")]
        public int Inherits { get; set; }
    }

    [PDFParsableComponent("Invalid-Code")]
    public class InvalidAttributeCode
    {
        /// <summary>
        /// Will throw execption as code-file is a reserved attribute name
        /// </summary>
        [PDFAttribute("code-file")]
        public int CodeFile { get; set; }
    }

}


// another class in different namesapce

namespace Scryber.Core.UnitTests.Generation.Fakes.More
{
    [PDFParsableComponent("DifferentNS")]
    public class ParserDifferentComplex : ParserInnerBase
    {
        /// <summary>
        /// A simple value element
        /// </summary>
        [PDFElement("Simple")]
        public Scryber.Drawing.PDFThickness Thickness { get; set; }
    }
}