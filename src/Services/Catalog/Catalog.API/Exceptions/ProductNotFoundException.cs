namespace Catalog.API.Exceptions;

/// <summary>
/// 找不到對應的產品。<br />
/// 僅複寫錯誤訊息。
/// </summary>
public class ProductNotFoundException : Exception
{
    /// <summary>
    /// 創建一個找不到對應的產品的例外實例。
    /// </summary>
    public ProductNotFoundException() : base("Product not found!") { }
}
