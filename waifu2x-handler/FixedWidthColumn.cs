using System;

using System.Collections.Generic;

using System.Text;

using System.Windows.Controls;

using System.Windows;



namespace CustomizeControls

{

    public class FixedWidthColumn : GridViewColumn

    {

        #region Constructor



        static FixedWidthColumn()

        {

            WidthProperty.OverrideMetadata(typeof(FixedWidthColumn),

                new FrameworkPropertyMetadata(null, new CoerceValueCallback(OnCoerceWidth)));

        }



        private static object OnCoerceWidth(DependencyObject o, object baseValue)

        {

            FixedWidthColumn fwc = o as FixedWidthColumn;

            if (fwc != null)

                return fwc.FixedWidth;

            return 0.0;

        }



        #endregion



        #region FixedWidth



        public double FixedWidth

        {

            get { return (double)GetValue(FixedWidthProperty); }

            set { SetValue(FixedWidthProperty, value); }

        }



        public static readonly DependencyProperty FixedWidthProperty =

            DependencyProperty.Register("FixedWidth", typeof(double), typeof(FixedWidthColumn),

            new FrameworkPropertyMetadata(double.NaN, new PropertyChangedCallback(OnFixedWidthChanged)));



        private static void OnFixedWidthChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)

        {

            FixedWidthColumn fwc = o as FixedWidthColumn;

            if (fwc != null)

                fwc.CoerceValue(WidthProperty);

        }



        #endregion

    }

}