using System;
using Scryber;
using Scryber.Components;
using Scryber.Data;

namespace Scryber.Handlebar.Components;


[PDFParsableComponent("With")]
public class WithHelper : BindingTemplateComponent
{

    private object _data;
    [PDFAttribute("data-bind", BindingOnly = true)]
    public object Data
    {
        get { return _data;}
        set { _data = value; }
    }

    [PDFElement("Content")]
    public WithContentHelper WithContent
    {
        get;
        set;
    }
    
    [PDFElement("ElseContent")]
    public WithElseHelper ElseTemplate
    {
        get;
        set;
    }
    
    private static readonly DataBindingBehaviour withBehaviour = new DataBindingBehaviour(
        enumerate: false,
        expandObject: false,
        setContextData: false,
        incrementIndex: false);

    public WithHelper() : this(ObjectTypes.NoOp, withBehaviour)
    {
        
    }
    
    protected WithHelper(ObjectType type, DataBindingBehaviour behaviour)
        : base(type, behaviour)
    {
    }
    

    protected override void DoBindDataIntoContainer(IContainerComponent container, int containerposition, DataContext context)
    {
        bool pop = false;
        if (null != this.Data)
        {
            var source = context.DataStack.HasData ? context.DataStack.Source : null;
            context.DataStack.Push(this.Data, context.DataStack.Source);
            pop = true;
        }

        try
        {
            base.DoBindDataIntoContainer(container, containerposition, context);
        }
        finally
        {
            if (pop)
            {
                context.DataStack.Pop();
            }
        }

        
    }
    
    protected override ITemplate GetTemplateForBinding(DataContext context, int index, int count)
    {
        var data = this.Data;
        
        if (null == data || (data is bool b && !b))
        {
            //our with is false, so if we have the optional else template push that
            if (null != this.ElseTemplate)
            {
                return this.ElseTemplate.Template;
            }
        }
        
        //we have data or no else option
        
        if (null == this.WithContent)
            throw new NullReferenceException(
                "No With Template was specified. This can be an empty template, but it is required.");
        
        return this.WithContent.Template;
    }
}


[PDFParsableComponent("WithContent")]
public class WithContentHelper
{
    [PDFTemplate]
    [PDFElement("")]
    public ITemplate Template { get; set; }  
}


[PDFParsableComponent("ElseContent")]
public class WithElseHelper
{
    [PDFTemplate]
    [PDFElement("")]
    public ITemplate Template { get; set; }  
}