   M S B A M L   `   `   `  ����L  HSIVIRE_Rehabilita, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullO,clr-namespace:SIVIRE_Rehabilita.UserControlsSIVIRE_Rehabilita.UserControls  5      A%clr-namespace:SIVIRE_Rehabilita.ModelSIVIRE_Rehabilita.Model  5   �   #    SIVIRE_Rehabilita.Window_Menu   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   d `Microsoft.Kinect.Wpf.Controls, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e350k(http://schemas.microsoft.com/kinect/2014  5   	   ?UserControls,clr-namespace:SIVIRE_Rehabilita.UserControls  5      c _Microsoft.Expression.Drawing, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35=ed4http://schemas.microsoft.com/expression/2010/drawing  5      :d2http://schemas.microsoft.com/expression/blend/2008  6C   Dmc;http://schemas.openxmlformats.org/markup-compatibility/2006  6�   1Model%clr-namespace:SIVIRE_Rehabilita.Model  6�   -   5          �Name$  window��5   �    � WindowStyle$
 None=�5      $	��768��6   $
��1366��6#    � WindowState$ 	Maximized=�60    � 
ResizeMode$ NoResize=�6v      SIVIRE_GradientBrush#���   6�    � 	Resources 5        showNavigationBar �� TargetProperty �� 
TargetName<  6System.Windows.Media.Animation.EasingThicknessKeyFrame p� KeyTime p� Value9  3System.Windows.Media.Animation.EasingDoubleKeyFrame	 O� KeyTime
 O� Value  closeNavigationBar  
showDialog R� KeyTime  System.Windows.Visibility   Visible R� Value  closeDialog   Hidden �� 
TargetType �� 	Resources �� Property �� Value��%�  &	       &	 E    &	 o    &	     '_� �    �� 5	   
   .��5
      r� $ (FrameworkElement.Margin)�6/   $ navigationBar��6e   .	�5        $ 0��6*   $ -40,20,0,20q�66     5      $ 0:0:0.4��6*   $
 0,20q�6<   5      U� 5      $ (UIElement.Opacity)�6,   $ navigationBar��6\   .\�5        $	 0��6'   $
 0Q�63     5      $	 0:0:0.4��6'   $
 1Q�69   5      r� 5      $ (FrameworkElement.Margin)�6/   $ navigationBar_Location��6e   .	�5        $ 0��6*   $ 	0,-15,0,0q�66     5      $ 0:0:0.4��6*   $ 
70,-15,0,0q�6<   5      5      �� 5   
   .��5      U� $ (UIElement.Opacity)�6,   $ navigationBar��6\   .\�5        $	 0:0:0.3��6'   $
 0Q�69   5      r� 5      $ (FrameworkElement.Margin)�6/   $ navigationBar��6e   .	�5        $ 0:0:0.3��6*   $ -40,20,0,20q�6<   5      5      �� 5   
   .��5       U� $, &(UIElement.Effect).(BlurEffect.Radius)�6,   $ kinectRegion_Content��6o   .\�5!        $	 0:0:0.5��6'   $
 9Q�69   5"      T� 5#      $ (UIElement.Visibility)�6,   $ hideGrid��6_   ..�5$      i� $ 0��6)   # Z 65   5%      U� 5&      $ (UIElement.Opacity)�6,   $ hideGrid��6\   .\�5'        $	 0:0:0.5��6'   $
 1Q�69   5(      5)      �� 5*   
   .��5+      U� $, &(UIElement.Effect).(BlurEffect.Radius)�6,   $ kinectRegion_Content��6o   .\�5,        $	 0:0:0.3��6'   $
 0Q�69   5-      T� 5.      $ (UIElement.Visibility)�6,   $ hideGrid��6_   ..�5/      i� $ 0:0:0.3��6)   # Z 6;   50      U� 51      $ (UIElement.Opacity)�6,   $ hideGrid��6\   .\�52        $	 0:0:0.3��6'   $
 0Q�69   53      54      �� 55   
   " _�6    56      ���� 57      '��       " ��6    58      ���� 59      '��       " ��6    .�5:      ��  � ��$
 Wrap=�6=   5;      5<      5=      5>      .�5?      ��  � ��$ 20(�6)   5@      5A      .��5C      0  *Microsoft.Kinect.Wpf.Controls.KinectRegion  -   5          Name$ kinectRegion��5C      $��20q�6+   .��5D   
   � -   5         �Name$ window_Content��5D      .+�5E      � -   5        $ kinectRegion_Content��5E       J� Effect 5F      -  'System.Windows.Media.Effects.BlurEffect  5G         Radius$ 0Q�6!    � ColumnDefinitions 5I      �� 5J      $
��161* �6'   �� 5K      $
��522* �6'    � RowDefinitions 5M      �� 5N      $	��130 �6$   �� 5O      $��* �6$   �� 5P      $	��250 �6$   .+�5S      4  .Microsoft.Kinect.Wpf.Controls.KinectUserViewer  -   5          Name$ 
userViewer��5S   F   $��0��6%   $��2��62   $��Center=�6Z   $y�Hidden=�6w   �� 5T      $��0��6   $��0��6%   $��Right=�65   $��Center=�6Q   $��Images/logo_SIVIRE.png��6l   $��	0,0,20,20q�6�    5U      3  -System.Windows.Media.Effects.DropShadowEffect  5V      5X      � 5Z      -   5        $ navigationBar��5Z      $��0��6.   $��1��6;   $
��Left=�6K   $
��0,20q�6f    J� Opacity$ 0Q�6t    5[        5\      .+�5^      �� $
�� ��6     SIVIRE_GrayBrush#���  60   $��80(�6`   $��Segoe UI Symbol)�6n   $��	0,-30,0,0q�6�   $
��Bold"�6�   �� 5_      -   5         ��Name$ navigationBar_Location��5_      $��Location��6<   
�������$��70(�6b   $��
70,-15,0,0q�6p   5`      �� 5b      -   5         ��Name$ btn_Back��5b       � ToolTip$ Volver atrás��6+     BackButtonStyle#���  6B   $	��Top=�6l   $��Right=�6�   $��1��6�   $	��130��6�   $	��130��6�   $y�Hidden=�6�   �� 5c      -	   5        $ btn_Exit��5c      $ Cierra la aplicación��6+     CloseButtonStyle#���  6K   $	��Top=�6v   $��Right=�6�   $��1��6�   $	��130��6�   $	��130��6�   �� 5e      $��0��6   $��1��6-   $��30,20q�6:    �� Orientation$ Vertical=�6I   $	��Top=�6`   .+�5f      �� -
   5        $ btn_userProfile��5f   E   $# Ir a información del usuario��6     KinectTileButton#���  6^   $	��200��6�   $	��200��6�   .��5g      � -   5        $
 grid��5g      
������� 5h        5i   "    J� OpacityMask 5k      
� 5l   "   .��5m   &   �   � Color$  	#00000000��63   ! � Offset$! 1Q�6E   � 5n   &   $  White��63   $! 0.9999Q�6A   5o   #   .+�5q      �� -   5        " ��Name$" img_user��5q   $   ��66   �� *Photo+A� 5r       	 SIVIRE_GreenBrush#��� 	 6&   $��6��6S   5s      5t      �� 5u      
�������$��40(�6H   $
��Bold"�6V   $��Center=�6h   ��6   �� *Name+ 5v        5w      5y      �� 5z      -   5        $ 
btn_logout��5z      $��40(�61   $	��0,5q�6?   $	��120��6a   $y�Hidden=�6n   .��5{      ��  
 SIVIRE_DarkGreenBrush#��� 
 6%   .+�5|      �� $	����6(     ButtonIconStyle#���  68   $��Segoe UI Symbol)�6b   �� 5}      $��Cerrar sesión��6(     ButtonTextStyle#���  6=   $��Segoe UI Symbol)�6g   5~      5      5�      �� 5�      $��0��6   $��3��6/   $
��0,20q�69   $��1��6G   � 5�      -   5        $ navigationRegion��5�      $��1��61   $��2��6>   $��30,20,0,-95q�6N   � 5�      -   5        $ navigationRegionHigh��5�      $��1��65   $��2��6B   $��2��6S   $��30,20q�6c   � 5�      -   5        $ exerciseInfoRegion��5�      $��1��63   $��2��6@   $��-20,20q�6T   ?   9SIVIRE_Rehabilita.UserControls.Menu_ExercisesScrollViewer  5�      -   5        #  Name$# exercisesScrollViewer��5�   :   ?	   9SIVIRE_Rehabilita.UserControls.Menu_ExercisesScrollViewer$ 	  ExerciseSelected$$ exerciseSelected��6�   $��2��6Y   $��2��6f   $y�Hidden=�6z   6�   5�      � 5�      -   5        $ hideGrid��5�      $	��-20q�6%   $��2��62   $��3��6F   $y�Hidden=�6W   ���	#4C000000$ 0Q�6�   5�      5�      5�      