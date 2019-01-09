using System.Globalization;
using System.Resources;

namespace stereopoly
{
  public interface ILocalize
  {
    CultureInfo GetCurrentCultureInfo();
    void SetLocale(CultureInfo ci);
  }
}
