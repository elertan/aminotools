using System;
using System.Collections.Generic;
using System.Text;
using AminoTools.iOS.CustomRenderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ViewCell), typeof(NavigationViewCellRenderer))]
namespace AminoTools.iOS.CustomRenderers
{
    public class NavigationViewCellRenderer : ViewCellRenderer
    {
        public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
        {
            var cell = base.GetCell(item, reusableCell, tv);
            switch (item.StyleId)
            {
                case "none":
                    cell.Accessory = UIKit.UITableViewCellAccessory.None;
                    break;
                case "checkmark":
                    cell.Accessory = UIKit.UITableViewCellAccessory.Checkmark;
                    break;
                case "detail-button":
                    cell.Accessory = UIKit.UITableViewCellAccessory.DetailButton;
                    break;
                case "detail-disclosure-button":
                    cell.Accessory = UIKit.UITableViewCellAccessory.DetailDisclosureButton;
                    break;
                case "disclosure":
                    cell.Accessory = UIKit.UITableViewCellAccessory.DisclosureIndicator;
                    break;
            }
            return cell;
        }
    }
}
