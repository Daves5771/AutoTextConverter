using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace AutoTextConverter
{
    public class ColorListView: ListView
    {
        List<SubItemState> itemStates;
        SubItemState itemState;

        public ColorListView()
        {
            itemStates = new List<SubItemState>();
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }
        protected override void OnDrawSubItem(DrawListViewSubItemEventArgs e)
        {
            TextFormatFlags flags = TextFormatFlags.SingleLine;

            using (StringFormat sf = new StringFormat())
            {
                // Store the column text alignment, letting it default
                // to Left if it has not been set to Center or Right.
                switch (e.Header.TextAlign)
                {
                    case HorizontalAlignment.Center:
                        sf.Alignment = StringAlignment.Center;
                        flags = TextFormatFlags.HorizontalCenter;
                        break;
                    case HorizontalAlignment.Right:
                        sf.Alignment = StringAlignment.Far;
                        flags = TextFormatFlags.Right;
                        break;
                    case HorizontalAlignment.Left:
                        sf.Alignment = StringAlignment.Near;
                        flags = TextFormatFlags.Left;
                        break;
                }
/*
                // Draw the text and background for a subitem with a 
                // negative value. 
                double subItemValue;
                if (e.ColumnIndex > 0 && Double.TryParse(
                    e.SubItem.Text, NumberStyles.Currency,
                    NumberFormatInfo.CurrentInfo, out subItemValue) &&
                    subItemValue < 0)
                {
                    // Unless the item is selected, draw the standard 
                    // background to make it stand out from the gradient.
                    if ((e.ItemState & ListViewItemStates.Selected) == 0)
                    {
                        e.DrawBackground();
                    }

                    // Draw the subitem text in red to highlight it. 
                    e.Graphics.DrawString(e.SubItem.Text,
                        listView1.Font, Brushes.Red, e.Bounds, sf);

                    return;
                }

                */

                // Unless the item is selected, draw the standard 
                // background to make it stand out from the gradient.
                if ((e.ItemState & ListViewItemStates.Selected) == 0)
                {
                    e.DrawBackground();
                    e.Graphics.DrawString(e.SubItem.Text,
                        this.Font, Brushes.Red, e.Bounds, sf);

                }
                else
                {
                    // Draw the subitem text in red to highlight it. 
                    e.Graphics.DrawString(e.SubItem.Text,
                        this.Font, Brushes.White, e.Bounds, sf);
                    ////     FlickerlessDrawSubItem(e.Graphics, e, sf);
                }

            }
        }

        protected override void OnDrawItem(DrawListViewItemEventArgs e)
        {
            Rectangle selRect = new Rectangle(14, e.Bounds.Y, e.Bounds.Width - 14, 15);
            if ((e.State & ListViewItemStates.Selected) != 0)
            {
                // Draw the background and focus rectangle for a selected item.
                e.Graphics.FillRectangle(Brushes.CornflowerBlue, e.Bounds);
                e.DrawFocusRectangle();
            }
            else
            {
                SolidBrush brush = new SolidBrush(BackColor);
                e.Graphics.FillRectangle(brush, selRect);

            }

            // Draw the item text for views other than the Details view.
            if (this.View != View.Details)
            {
                e.DrawText();
            }
        }

        protected override void OnDrawColumnHeader(DrawListViewColumnHeaderEventArgs e)
        {

            using (StringFormat sf = new StringFormat())
            {
                // Store the column text alignment, letting it default
                // to Left if it has not been set to Center or Right.
                switch (e.Header.TextAlign)
                {
                    case HorizontalAlignment.Center:
                        sf.Alignment = StringAlignment.Center;
                        break;
                    case HorizontalAlignment.Right:
                        sf.Alignment = StringAlignment.Far;
                        break;
                    case HorizontalAlignment.Left:
                        sf.Alignment = StringAlignment.Near;
                        break;
                }

                // Draw the standard header background.
                e.DrawBackground();

                // Draw the header text.
                using (Font headerFont =
                            new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular))
                {
                    e.Graphics.DrawString(e.Header.Text, headerFont,
                        Brushes.Black, e.Bounds, sf);
                }


                // Draw the standard header background.
                //      e.DrawBackground();

                /*
                // Draw the header text.
                using (Font headerFont =
                            new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular))
                {
                    e.Graphics.DrawString(e.Header.Text, headerFont,
                        Brushes.Black, e.Bounds);
                }*/
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            ListViewItem item = this.GetItemAt(e.X, e.Y);
            if (item != null)
                this.Invalidate(item.Bounds);
            base.OnMouseMove(e);
        }

        private void AlternativeDrawSubItem(DrawListViewSubItemEventArgs e)
        {
            e.Graphics.DrawString(Name, new Font(FontFamily.GenericSansSerif, 8.5F, FontStyle.Regular), new SolidBrush(Color.Red), new Point(16, e.Bounds.Y + 1));
        }

        private void AlternativeDrawItem(DrawListViewItemEventArgs e)
        {
            e.Graphics.DrawString(Name, new Font(FontFamily.GenericSansSerif, 8.5F, FontStyle.Regular), new SolidBrush(Color.Red), new Point(16, e.Bounds.Y + 1));
        }

        private void ModifiedDrawSubItem(DrawListViewSubItemEventArgs e)
        {
            Rectangle selRect = new Rectangle(14, e.Bounds.Y, e.Bounds.Width - 14, 15);
            if (e.ItemState  == ListViewItemStates.Selected)
            {
                e.Graphics.FillRectangle(Brushes.CornflowerBlue, selRect);
                e.Graphics.DrawString(e.SubItem.Text, new Font(FontFamily.GenericSansSerif, 8.5F, FontStyle.Regular), new SolidBrush(Color.White), new Point(18, e.Bounds.Y + 0));
                // Draw the focus rectangle around the selected item.
               // ControlPaint.DrawFocusRectangle(e.Graphics, selRect);
            }
            else
            {
                // Otherwise, draw the rectangle filled with the 
                // background color of the control
                SolidBrush brush = new SolidBrush(BackColor);
                e.Graphics.FillRectangle(brush, selRect);
                e.Graphics.DrawString(e.SubItem.Text, new Font(FontFamily.GenericSansSerif, 8.5F, FontStyle.Regular), new SolidBrush(Color.Blue), new Point(18, selRect.Y));
            }
        }

        private void ModifiedDrawItem(DrawListViewItemEventArgs e)
        {
            /*
            if (e.Index != -1 && e.Index < itemStates.Count)
            {
                itemState = (ItemState)itemStates[e.Index];
                Rectangle selRect = new Rectangle(14, e.Bounds.Y, e.Bounds.Width - 14, 15);
                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                {
                    e.Graphics.FillRectangle(Brushes.CornflowerBlue, selRect);
                    e.Graphics.DrawString(Items[e.Index].ToString(), new Font(FontFamily.GenericSansSerif, 8.5F, FontStyle.Regular), new SolidBrush(Color.White), new Point(18, e.Bounds.Y + 0));
                    // Draw the focus rectangle around the selected item.
                    ControlPaint.DrawFocusRectangle(e.Graphics, selRect);
                }
                else
                {
                    // Otherwise, draw the rectangle filled with the 
                    // background color of the control
                    SolidBrush brush = new SolidBrush(BackColor);
                    e.Graphics.FillRectangle(brush, selRect);
                    e.Graphics.DrawString(Items[e.Index].ToString(), new Font(FontFamily.GenericSansSerif, 8.5F, FontStyle.Regular), new SolidBrush(itemState.color), new Point(18, selRect.Y));
                }
            }*/
        }

  
        public void AddSubItem(int itemNumber, int subItemNumber, string s, Color color)
        {
            itemState = new SubItemState();
            itemState.color = color;
            itemState.Text = s;
         //   item.SubItems.Add("");
            itemStates.Add(itemState);
         //   statusListView.Items[statusListView.Items.Count - 1].SubItems[subItemNumber].Text = s;
  //          this.Items.Add(itemState);
        }

        public void AddSubItem(int itemNumber, int subItemNumber, SearchStruct searchInfo, Color color)
        {
           itemState = new SubItemState();
           itemState.color = color;
           itemState.Text = searchInfo.Description;
           itemStates.Add(itemState);
        }

        public SubItemState GetSubItemState(int index)
        {
            return itemStates[index];
        }

        protected override void OnItemCheck(ItemCheckEventArgs ice)
        {
            base.OnItemCheck(ice);
        }

    }
    public class SubItemState
    {
        public bool bCheckState;
        public Color color;
        public string text;
        public bool bSelected;
        public SearchStruct searchInfo;

        public SubItemState()
        {
        }

        public SubItemState(string text, bool itemChecked)
        {
            this.text = text;
        }

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public SearchStruct SearchInfo
        {
            get {return searchInfo;}
        }

        public override string ToString()
        {
            if (Text == "")
                return base.ToString();
            else return Text;
        }

    }
}



      
