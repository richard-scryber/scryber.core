using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Drawing;
using Scryber.Text;
using Scryber.Components;
namespace Scryber.UnitSamples;

using Scryber.Html.Components;

[TestClass]
public class TestCode
{
    [TestMethod]
    public void JustATest()
    {
        

        //documents can either contain a body **or** a frameset.
        //Not both.
        var doc = new HTMLDocument();
        doc.Body = new HTMLBody();
        doc.Body.Contents.Add("All the content goes here.");


    }
}