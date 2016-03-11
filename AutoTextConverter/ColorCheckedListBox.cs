using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.Drawing;
// SaelSoft -- ColorCheckedListBox.cs
// Purpose --  Subclass of CheckedListBox to support colors
// Copywrite 2008 David Saelman

namespace SaelSoft.AutoTextConverter
{
    public class ColorCheckedListBox: CheckedListBox
    {
        List<ItemState> itemStates;
        ItemState itemState;
        SolidBrush selectedColorBrush;

        public ColorCheckedListBox()
        {
            itemStates = new List<ItemState>();
            selectedColorBrush = new SolidBrush(SystemColors.ActiveCaption);
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {

            if (e.Index != -1)
            {
                base.OnDrawItem(e);

                if (Items.Count > 0)
                {
                    ModifiedDrawItem(e);
                }
                else
                    AlternativeDrawItem(e);
            }
        }

        private void AlternativeDrawItem(DrawItemEventArgs e)
        {
            e.Graphics.DrawString(Name, new Font(FontFamily.GenericSansSerif, 8.5F, FontStyle.Regular), new SolidBrush(Color.Red), new Point(16, e.Bounds.Y + 1));
        }

        private void ModifiedDrawItem(DrawItemEventArgs e)
        {
            if (e.Index != -1 && e.Index < itemStates.Count)
            {
                itemState = (ItemState)itemStates[e.Index];
                Rectangle selRect = new Rectangle(14, e.Bounds.Y, e.Bounds.Width - 14, 15);
                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                {
                    e.Graphics.FillRectangle(selectedColorBrush, selRect);
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
            }
        }

        public void Add(string s)
        {
            this.Items.Add(s);
            itemState = new ItemState();
            itemState.Text = s;
            itemStates.Add(itemState);
            this.Items.Add(itemState);
        }

        public void Add(string s, Color color)
        {
            itemState = new ItemState();
            itemState.color = color;
            itemState.Text = s;
            itemStates.Add(itemState);
            this.Items.Add(itemState);
        }

        public void Add(string s, bool state, Color color)
        {
            itemState = new ItemState();
            itemState.color = color;
            itemState.Text = s;
            itemState.bCheckState = state;
            itemStates.Add(itemState);
            this.Items.Add(itemState, state);
        }

        public void Add(SearchStruct searchInfo, bool state, Color color)
        {
            itemState = new ItemState();
            itemState.color = color;
            itemState.Text = searchInfo.Description;
            itemState.searchInfo = searchInfo;
            itemState.bCheckState = state;
            itemStates.Add(itemState);
            this.Items.Add(itemState, state);
        }

        protected override void OnMeasureItem(MeasureItemEventArgs e)
        {
            base.OnMeasureItem(e);
            if (Items.Count > 0)
                e.ItemHeight += 2;
        }

        protected override void OnItemCheck(ItemCheckEventArgs ice)
        {
            base.OnItemCheck(ice);
        }
    }

    public class ItemState
    {
        public bool bCheckState;
        public Color color;
        public string text;
        public bool bSelected;
        public SearchStruct searchInfo;
        
        public ItemState()
        {
        }

        public ItemState(string text, bool itemChecked)
        {
            this.text = text;
            this.Checked = itemChecked;
        }

        public bool Checked
        {
            get { return bCheckState; }
            set { bCheckState = value; }
        }

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public SearchStruct SearchInfo
        {
            get { return searchInfo; }
        }

        public override string ToString()
        {
            if (Text == "")
                return base.ToString();
            else return Text;
        }
    }
}
