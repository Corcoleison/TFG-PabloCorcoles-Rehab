   M S B A M L   `   `   `  ����  SIVIRE_Rehabilita$    SIVIRE_Rehabilita.Window_Login   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     6F   -   5          � WindowStyle$
  None=�5   	   $	��768��6   $
��1366��6)    � WindowState$ 	Maximized=�66    � 
ResizeMode$ NoResize=�6}      SIVIRE_GradientBrush#���   6�    � 	Resources 5        
showDialog �� TargetProperty �� 
TargetName9  3System.Windows.Media.Animation.EasingDoubleKeyFrame O� KeyTime O� Value R� KeyTime  System.Windows.Visibility	   Visible
 R� Value  closeDialog   Hidden��%`  &	       &	 �    �� 5      .��5      U� $, &(UIElement.Effect).(BlurEffect.Radius)�6#   $ window_subContent��6f   .\�5	        $ 0:0:0.5��6   $ 9Q�6-   5
      T� 5      $ (UIElement.Visibility)�6#   $ hideGrid��6V   ..�5      i� $ 0��6   #
 Z	 6)   5      U� 5      $ (UIElement.Opacity)�6#   $ hideGrid��6S   .\�5        $ 0:0:0.5��6   $ 1Q�6-   5      5      �� 5   
   .��5      U� $, &(UIElement.Effect).(BlurEffect.Radius)�6,   $ window_subContent��6o   .\�5        $ 0:0:0.3��6'   $ 0Q�69   5      T� 5      $ (UIElement.Visibility)�6,   $ hideGrid��6_   ..�5      i� $ 0:0:0.3��6)   #
 Z 6;   5      U� 5      $ (UIElement.Opacity)�6,   $ hideGrid��6\   .\�5        $ 0:0:0.3��6'   $ 0Q�69   5      5      5      .��5      � -   5         �Name$ window_Content��5      .+�5      � -   5        $ window_subContent��5   	    J� Effect 5       -  'System.Windows.Media.Effects.BlurEffect  5!         Radius$ 0Q�6    � ColumnDefinitions 5#      �� 5$      $	��50* �6   �� 5%      $	��50* �6   .+�5(      �� -   5         ��Name$ btn_Exit��5(        CloseButtonStyle#���  6   $	��Top=�6I   $��Right=�6t   $��1��6�   $	��130��6�   $	��130��6�   �� 5*      $��0��6   $��100,0,50,50q�6   $��Right=�60   $��Center=�6L   $��Images/logo_SIVIRE.png��6g    5+      3  -System.Windows.Media.Effects.DropShadowEffect  5,      5.      �� 50      $��0��6   $��3��6"   $��0,150q�6,   � 51      $��1��6
   $��50,0,0,0q�6   .+�52      �� $	��300��6   $
��Left=�6   $��Center=�68   .+�53      �� -   5         ��Name$ tb_nickName��53      $��Usuario��6$   $��32(�63   $��55,5,0,5q�6A   ���	#FFE4E4E4���	#4CFFFFFF �� SelectionBrush
 �   �$	��250��6�   $��55��6�   ��6�   Z� 6�   G� 54      -   5         G�Name$ tb_password��54   "    G� Password$ ******��67   $��32(�6I   $��55,5,0,5q�6W   ���	#FFE4E4E4���	#4CFFFFFF G� SelectionBrush
 �   �$��55��6�   $	��248��6�   ��6�   Z� 6�   �� 55      -   5         ��Name$ cb_remember��55      $��
Recordarme��6%   $
��Left=�6:   $	��0,5q�6U   $	��250��6b   $��24(�6n   
�������$��Center=�6�   �� 56      -   5        $��40(�56      $	��0,5q�6   $��55��6=   .��57      �   SIVIRE_DarkGreenBrush#���  6   .+�58   	   �� $��Entrar��6     ButtonTextStyle#���  6!   59   	   5:      5;      �� 5<      $��50��6   $
��Left=�6   $��Center=�67   $��	0,0,0,106q�6R   .+�5=      �� $
��👤��6   $��40(�6   $	��0,5q�6(   
����������	#4CFFFFFF$��Segoe UI Symbol)�6_   $��55��6|   $��50��6�   �� 5>      $
��🔒��6   $��40(�6   $	��0,5q�6(   
����������	#4CFFFFFF$��Segoe UI Symbol)�6_   $��55��6|   $��50��6�   5?      5@      � 5A      -	   5        $ hideGrid��5A   
   $��2��6   $y�Hidden=�60   ���	#4C000000 J� Opacity$ 0Q�6[   5B      5C      5D      