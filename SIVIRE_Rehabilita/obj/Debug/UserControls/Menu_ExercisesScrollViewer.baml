   M S B A M L   `   `   `  ����  SIVIRE_Rehabilita?    9SIVIRE_Rehabilita.UserControls.Menu_ExercisesScrollViewer   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      d `Microsoft.Kinect.Wpf.Controls, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e350k(http://schemas.microsoft.com/kinect/2014  5      Dmc;http://schemas.openxmlformats.org/markup-compatibility/2006  5      -   5          >�Name$  UserControl��5      $
��1000��5      $	��250��6#   $	��250��60    � 	Resources 5	         LeftRightButtonStyle �� 
TargetType �� Property �� Value0  *Microsoft.Kinect.Wpf.Controls.KinectRegion4  .System.Windows.Automation.AutomationProperties �� 
TargetType �Name �� Padding ��Name	 J� Opacity
 ��Name J� IsHitTestVisible �� StrokeEndLineCap �� StrokeDashArray �� StrokeDashOffset'  !System.Windows.VisualStateManager   VisualStateGroups%  System.Windows.VisualStateGroup   States   System.Windows.VisualState   
Storyboard �� 
TargetName �� TargetProperty R� KeyTime  MediumGrayBrush R� Value	 W� To g� Duration��%y
  &	        7  7  �� 5
   
   " ��6-   .�5      ��  � ��$ 48��6)   �� 5       � ��$ 48��6*   �� 5       � ��$ 5,5,5,5q�6(   �� 5       �@ KinectPressInset$ 5,5,5,5q�6@   �� 5       � ��$ Bottom=�62   �� 5       � ��$ Normal"�6+   �� 5       �@ AutomationId$ 
BackButton��6B   �� 5       �@ Name$
 Back��6:   �� 5       �@ ItemType$ Navigation Button��6>   �� 5       � �� 5      �� 5      " ��6&   .R�5      � $ RootGrid��6   .+�5      �� # z��6%   ���	#CC999999._�5   "   � $��	8,-30,0,0q�6'   .+�5   &   �� $ BackgroundGlyph��60   $	����6I   
���   �$	 0Q�6l   $��Segoe UI Symbol)�6x   $	��120(�6�   $��	-7,20,0,0q�6�   �� 5   &   $ NormalGlyph��60   $	����6E   
�������$��Segoe UI Symbol)�6h   $	��120(�6�   $��	-7,20,0,0q�6�   �� 5   &   $ 
ArrowGlyph��60   $	����6D   
�������$	 0Q�6g   $��Segoe UI Symbol)�6s   $	��120(�6�   $��	-7,20,0,0q�6�   5   #   5      �� 5      $
 FocusVisualWhite��5        .  
�������$ Square=�5#      $	 1,1R�5$      $	 0Q�5%      $	 1.5Q�5&      �� 5'      $
 FocusVisualBlack��5(       .  
���   �$ Square=�5+      $	 1,1R�5,      $	 0Q�5-      $	 0.5Q�5.       50        51   "   CommonStates��. 52   &     
Normal��62     53   &   PointerOver��. 54   *   �� .��55   .   T� $ BackgroundGlyph��6L   $ 
Foreground�6t   ..�56   2   i� $ 0��6I   8   6U   57   /   T� 58   .   $ NormalGlyph��6L   $ 
Foreground�6p   ..�59   2   i� $ 0��6I   8  6U   5:   /   5;   +   5<   '     5=   &   Pressed��. 5>   *   �� .��5?   .   W� $ 
ArrowGlyph��5@   -   $ Opacity�5A   -   $ 1X�5B   -   $ 0D�5C   -   W� 5D   .   $ BackgroundGlyph��5E   -   $ Opacity�5F   -   $ 1X�5G   -   $ 0D�5H   -   W� 5I   .   $ NormalGlyph��5J   -   $ Opacity�5K   -   $ 0X�5L   -   $ 0D�5M   -   5N   +   5O   '     5P   &   Disabled��. 5Q   *   �� .��5R   .   T� $ RootGrid��6L   $ 
Visibility�6m   ..�5S   2   i� $ 0��6I   $ 	Collapsed��6U   5T   /   5U   +   5V   '   5W   #     5X   "   FocusStates��. 5Y   &     Focused��. 5Z   *   �� .��5[   .   W� $ FocusVisualWhite��5\   -   $ Opacity�5]   -   $ 1X�5^   -   $ 0D�5_   -   W� 5`   .   $ FocusVisualBlack��5a   -   $ Opacity�5b   -   $ 1X�5c   -   $ 0D�5d   -   5e   +   5f   '     5g   &   	Unfocused��62     5h   &   PointerFocused��62   5i   #   5k      5l      5n      5o      5p      .��5r      �� .+�5s   
   �� -   5         ��Name$ label��5s        HeaderStyle#��[ 6     SIVIRE_DarkGrayBrush#���  6�   $��0q�6�   
���������6D   �� *Routine.Name+ �� ElementName$ UserControl�� �� Mode$ OneWay=�6�   �� 5t   
   -   5         ��Name$ scrollViewer��5t      $
��Auto=�6-   $��Disabled=�6R      IsScrollInertiaEnabled . $��Bottom=�6�   .��5u      �� -   5         ��Name$ itemsControl��5u       � ToolTip$ Iniciar este ejercicio��61     ExerciseItemTemplate#��[ 6R   ���	#BFFFFFFF��6�   �� *Routine.Exercises+$ UserControl����5v      �� 5w      .R�5x      � $��Center=�6$    � Orientation$ 
Horizontal=�6?   $��	50,0,50,0q�6X   5y      5{      5|      �� 5}   
   $��
0,-187,0,0q�6   .+�5~      �� -   5        #���   5~      $	��114��6D   $
��Left=�6c   $	��170��6~   �� 5      -   5        #���   5      $	��114��6D   $��Stretch=�6d   $��Right=�6�   $��
0,-170,0,0q�6�     J� RenderTransformOrigin$  0.5,0.5&�6�   $	��170��6�   z�5�      V� 5�      .~�5�      �� ! �� ScaleX$! -1Q�6)   �� 5�      �� 5�      T� 5�      5�      5�      5�      5�      5�      