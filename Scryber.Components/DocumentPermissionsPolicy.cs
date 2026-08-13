using System;
using System.Collections.Generic;
using System.Linq;
using Scryber.Components;

namespace Scryber;

/// <summary>
/// Defines the permissions policy for a document or an iFrame content
/// </summary>
[PDFParsableValue]
public class DocumentPermissionsPolicy
{
    private Dictionary<PermissionPolicyType, PermissionsPolicy> _parsedGroup;
    
    public DocumentPermissionsPolicy()
    {
        _parsedGroup = new Dictionary<PermissionPolicyType, PermissionsPolicy>();
        
        foreach(var perm in _negativePermissions)
            _parsedGroup.Add(perm.Type, perm);
    }

    public void SetPolicy(PermissionPolicyType policyType, string options)
    {
        PermissionsPolicy policy;
        switch (policyType)
        {
            case PermissionPolicyType.DataPassthrough:
                policy = new PermissionDataPassthrough(options ?? "self");
                break;
                case PermissionPolicyType.StylePassthrough:
                    policy = new PermissionStylePassthrough(options ?? "self");
                    break;
                case PermissionPolicyType.InnerStyles:
                    policy = new PermissionAllowInnerStyles(options ?? "self");
                    break;
                case PermissionPolicyType.InnerLink:
                    policy = new PermissionAllowInnerLink(options ?? "self");
                    break;
                case PermissionPolicyType.InnerNavigation:
                    policy = new PermissionAllowInnerNavigation(options ?? "self");
                    break;
                case PermissionPolicyType.InnerForms:
                    policy = new PermissionAllowInnerForms(options ?? "self");
                    break;
                case PermissionPolicyType.OuterHtml:
                    policy = new PermissionAllowOuterHtml(options ?? "self");
                    break;
                case PermissionPolicyType.InnerFrames:
                    policy = new PermissionAllowInnerFrames(options ?? "self");
                    break;
                case PermissionPolicyType.InnerImages:
                    policy = new PermissionAllowInnerImages(options ?? "self");
                    break;
                case PermissionPolicyType.InnerInlineStyle:
                    policy = new PermissionAllowInlineStyles(options ?? "self");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(policyType), policyType, null);
        }
        _parsedGroup[policy.Type] = policy;
    }

    public void SetPolicy(PermissionsPolicy policy)
    {
        if(null ==policy)
            throw new ArgumentNullException(nameof(policy));
        
        _parsedGroup[policy.Type] = policy;
    }
    
    public PermissionsPolicy GetPolicy(PermissionPolicyType type)
    {
        if(this._parsedGroup.TryGetValue(type, out var policy))
            return policy;
        
        throw new KeyNotFoundException($"The type {type} does not exist");
    }

    public bool IsAllowed(PermissionPolicyType type, string value)
    {
        var policy = _parsedGroup[type];
        
        if(null == policy)
            throw new NullReferenceException($"The type {type} does not exist");

        return policy.IsAllowed(value);

    }


    public static DocumentPermissionsPolicy Parse(string str)
    {
        DocumentPermissionsPolicy policy = new DocumentPermissionsPolicy();
        if (!string.IsNullOrEmpty(str))
        {
            var each = str.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (each.Length > 0)
            {
                foreach (var item in each)
                {
                    var one = ParsePermissionItem(item);
                    if (null != one)
                        policy.SetPolicy(one);
                }
            }

        }
        return policy;
    }

    private static PermissionsPolicy ParsePermissionItem(string str)
    {
        return PermissionsPolicy.Parse(str);
    }
    

    private static readonly List<PermissionsPolicy> _negativePermissions;

    static DocumentPermissionsPolicy()
    {
        _negativePermissions = new List<PermissionsPolicy>();
        _negativePermissions.Add(new PermissionDataPassthrough("none"));
        _negativePermissions.Add(new PermissionStylePassthrough("none"));
        _negativePermissions.Add(new PermissionAllowInnerNavigation("none"));
        _negativePermissions.Add(new PermissionAllowOuterHtml("none"));
        _negativePermissions.Add(new PermissionAllowInlineStyles("none"));
        _negativePermissions.Add(new PermissionAllowInnerForms("none"));
        _negativePermissions.Add(new PermissionAllowInnerFrames("none"));
        _negativePermissions.Add(new PermissionAllowInnerImages("none"));
        _negativePermissions.Add(new PermissionAllowInnerLink("none"));
        _negativePermissions.Add(new PermissionAllowInnerStyles("none"));
    }
}


/// <summary>
/// All the available permission policy types that cen be allowed or enforces in a document or frame.
/// </summary>
public enum PermissionPolicyType
{
    /// <summary>
    /// Any existing data in the parameters or data stack can be blocked from being available to the inner content.
    /// </summary>
    DataPassthrough,
    
    /// <summary>
    /// Any defined styles in the parent can be blocked from affecting the output of the inner content.
    /// </summary>
    StylePassthrough,
    
    /// <summary>
    /// Defines the permission behaviour of `style` tags with inner definitions can be blocked from the document or frame content.
    /// NOTE: This is independent of OuterHTML
    /// </summary>
    InnerStyles,
    
    /// <summary>
    /// Defines the permission behaviour of `link` tags with their referenced sources can be blocked from the document or frame content.
    /// 
    /// </summary>
    InnerLink,
    /// <summary>
    /// Defines the permission behaviour of `a`nchor tags with navigation links that can be blanked from the document or frame content.
    /// </summary>
    InnerNavigation,
    
    /// <summary>
    /// Any `img` tags with inner references can be blocked from the document or frame content.
    /// </summary>
    InnerImages,
    
    /// <summary>
    /// Defines how whole document contents are handled. If allowed then the full content will be supported,
    /// otherwise only the contents in the body will be used. If `link` and `style` tags are in the header, then these will be included only if they are allowed. 
    /// </summary>
    OuterHtml,
    
    /// <summary>
    /// Defines the permission behaviour of tags/components with @style='..' values which can be stripped from the document or frame content.
    /// </summary>
    InnerInlineStyle,
    
    /// <summary>
    /// Defines the permission behaviour of inner frames (or embeds or objects) that can be removed from the document or frame content
    /// </summary>
    InnerFrames,
    
    /// <summary>
    /// Defines the permission behaviour of inner forms (or inputs, selects or buttons) that can be removed from the document or frame content
    /// </summary>
    InnerForms,
    
}

public abstract class PermissionsPolicy
{
    public const string AllowInnerStylesKey = "inner-style";
    public const string DataPassthroughKey = "data-passthrough";
    public const string StylesPassthroughKey = "style-passthrough";
    public const string AllowInnerLinkKey = "inner-link";
    public const string AllowInnerNavigationKey = "inner-navigation";
    public const string AllowInnerImagesKey = "inner-images";
    public const string AllowOuterHtmlKey = "outer-html";
    public const string AllowInlineStylesKey = "inline-styles";
    public const string AllowInnerFramesKey = "inner-frames";
    public const string AllowInnerFormsKey = "inner-forms";
    
    public PermissionPolicyType Type { get; private set; }
    
    protected string[] Options { get; set; }
    
    protected  PermissionsPolicy(PermissionPolicyType type, string options)
    {
        this.Type = type;
        this.Options = options.ToLowerInvariant().Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    }
    
    public virtual bool IsAllowed(string data)
    {
        if(this.Options.Length == 1 && this.Options[0] == "any")
            return true;
        
        if(this.Options.Length == 1 && this.Options[0] == "none")
            return false;
            
        if(data == "self" && this.Options.Contains("self"))
            return true;
        
        return false;
    }
    

    public static PermissionsPolicy Parse(string str)
    {
        PermissionsPolicy parsed = null;
        if (string.IsNullOrEmpty(str))
            return null;
        
        string typeStr = str.IndexOf(' ') > 0 ? str.Substring(0, str.IndexOf(' ')) : str;
        string options = (str.Length > typeStr.Length + 1) ? str.Substring(typeStr.Length + 1).Trim() : null;
        
        switch (typeStr.ToLower())
        {
            case AllowInnerStylesKey:
                parsed = new PermissionAllowInnerStyles(options ?? "self");
                break;
            case DataPassthroughKey:
                parsed = new PermissionDataPassthrough(options ?? "self");
                break;
            case StylesPassthroughKey:
                parsed = new PermissionStylePassthrough(options ?? "self");
                break;
            case AllowInnerLinkKey:
                parsed = new PermissionAllowInnerLink(options ?? "self");
                break;
            case AllowInnerImagesKey:
                parsed = new PermissionAllowInnerImages(options ?? "self");
                break;
            case AllowInnerNavigationKey:
                parsed = new PermissionAllowInnerNavigation(options ?? "self");
                break;
            case AllowOuterHtmlKey:
                parsed = new PermissionAllowOuterHtml(options ?? "self");
                break;
            case AllowInlineStylesKey:
                parsed = new PermissionAllowInlineStyles(options ?? "self");
                break;
            case AllowInnerFramesKey:
                parsed = new PermissionAllowInnerFrames(options ?? "self");
                break;
            case AllowInnerFormsKey:
                parsed = new PermissionAllowInnerForms(options ?? "self");
                break;
            default:
                parsed = null;
                break;
        }
        return parsed;
    }
}

public class PermissionDataPassthrough : PermissionsPolicy
{
    public PermissionDataPassthrough(string options) : base(PermissionPolicyType.DataPassthrough, options)
    {}
    
}

public class PermissionStylePassthrough : PermissionsPolicy
{
    public PermissionStylePassthrough(string options) : base(PermissionPolicyType.StylePassthrough, options)
    {}
}

public class PermissionAllowInnerStyles : PermissionsPolicy
{
    public PermissionAllowInnerStyles(string options) : base(PermissionPolicyType.InnerStyles, options)
    {}
}

public class PermissionAllowInnerLink : PermissionsPolicy
{
    public PermissionAllowInnerLink(string options) : base(PermissionPolicyType.InnerLink, options)
    {}
}
public class PermissionAllowOuterHtml : PermissionsPolicy
{
    public PermissionAllowOuterHtml(string options) : base(PermissionPolicyType.OuterHtml, options)
    {}
}
public class PermissionAllowInnerImages : PermissionsPolicy
{
    public PermissionAllowInnerImages(string options) : base(PermissionPolicyType.InnerImages, options)
    {}
}
public class PermissionAllowInnerNavigation : PermissionsPolicy
{
    public PermissionAllowInnerNavigation(string options) : base(PermissionPolicyType.InnerNavigation, options)
    {}
}

public class PermissionAllowInlineStyles : PermissionsPolicy
{
    public PermissionAllowInlineStyles(string options) : base(PermissionPolicyType.InnerInlineStyle, options)
    {}
}

public class PermissionAllowInnerFrames : PermissionsPolicy
{
    public PermissionAllowInnerFrames(string options) : base(PermissionPolicyType.InnerFrames, options)
    {}
}

public class PermissionAllowInnerForms : PermissionsPolicy
{
    public PermissionAllowInnerForms(string options) : base(PermissionPolicyType.InnerForms, options)
    {}
}