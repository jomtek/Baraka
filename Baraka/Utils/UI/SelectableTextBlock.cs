using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace Baraka.Utils.UI
{
    // TODO: WIP
    public partial class SelectableTextBlock : TextBlock
    {
        TextPointer StartSelectPosition;
        TextPointer EndSelectPosition;

        TextRange _ntr = null;

        public SelectableTextBlock()
        {
            
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            if (_ntr != null)
            {
                _ntr.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.Transparent);
            }

            Point mouseDownPoint = e.GetPosition(this);
            StartSelectPosition = this.GetPositionFromPoint(mouseDownPoint, true);
        }


        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);

            _ntr.ApplyPropertyValue(TextElement.BackgroundProperty, new SolidColorBrush(Colors.Transparent));
            _ntr = null;
            StartSelectPosition = null;
            EndSelectPosition = null;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (StartSelectPosition == null)
            {
                return;
            }

            Point mouseUpPoint = e.GetPosition(this);
            EndSelectPosition = this.GetPositionFromPoint(mouseUpPoint, true);

            _ntr = new TextRange(StartSelectPosition, EndSelectPosition);

            // change style
            _ntr.ApplyPropertyValue(TextElement.BackgroundProperty, new SolidColorBrush(Colors.DarkCyan));
        }
    }
}
