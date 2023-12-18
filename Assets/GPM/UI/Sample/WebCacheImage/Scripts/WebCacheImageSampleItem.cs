namespace Gpm.Ui.Sample
{
    //using Gpm.Common;
    using Gpm.Ui;
    using System.Text;
    using UnityEngine;
    using UnityEngine.UI;

    public class WebCacheImageSampleData : InfiniteScrollData
    {
        public WebCacheImageSampleData(string url)
        {
            this.url = url;
        }

        public string url;
    }

    public class WebCacheImageSampleItem : InfiniteScrollItem
    {
        public WebCacheImage cache;

        public override void UpdateData(InfiniteScrollData scrollData)
        {
            base.UpdateData(scrollData);

            WebCacheImageSampleData cacheData = scrollData as WebCacheImageSampleData;

            cache.SetLoadTextureEvent(OnLoadTexture);

            cache.SetUrl(cacheData.url);
        }

        public void OnLoadTexture(Texture tex)
        {
        }
    }

}