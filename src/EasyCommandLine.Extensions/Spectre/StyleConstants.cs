using Spectre.Console;

namespace EasyCommandLine.Extensions.Spectre;

public static class StyleConstants
{
    public static readonly Style StringStyle = new Color(205, 144, 105);
    public static readonly Style NumberStyle = new Color(180, 205, 168);
    public static readonly Style KeywordStyle = new Color(86, 156, 214);
    public static readonly Style BracketStyle = new(new Color(220, 220, 170), Color.Default, Decoration.Dim);
    public static readonly Style MemberStyle = new(new Color(156, 220, 254));
}