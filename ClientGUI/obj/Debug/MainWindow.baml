   M S B A M L   `   `   `  ����D  @ClientGUI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:ClientGUI	ClientGUI  5   	       ClientGUI.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:ClientGUI     5   	   U QMaterialDesignThemes.Wpf, Version=2.3.1.953, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	      MaterialDesignBody#���   5	   	   $��Regular"�5
   	   $��13(�5   	   &   System.Windows.Media.TextOptions    TextFormattingMode$  Ideal=�5   	      TextRenderingMode$
 Auto=�5   	    ) %MaterialDesignDarkSeparatorBackground#���  5   	     MaterialDesignFont#���  5   	    � Title$ 
MainWindow��5   	   $	��600��6   $	��800��6)   .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��263* �6    � ColumnDefinitions 5   
   �� 5      �� 5      $��2* �6   .+�5   
   (  "MaterialDesignThemes.Wpf.ColorZone     Mode$
 Dark=�6#   $��16q�6/   $��2��6<   $��66��6P   $��	0,0,0.2,0q�6\   .��5      ]� .+�5      '  !MaterialDesignThemes.Wpf.PopupBox  $��Right=�6*      PlacementMode$ BottomAndAlignRightEdges=�6A      PopupContent. 5      �� .@�5      �� -   5         ��Name$
 Test��5   !   .��6B   Hello World6O   �� 5       -   5        $ Loign��5    !   .��6T   Login6[   �� 5!      -   5        $
 AddF��5!   !   $��
Add friend��6/   6[   �� 5"      -   5        $��chattest��5"   !   .��6J   
chattest6T   5#      5$      �� 5%      	 �� Orientation$	 
Horizontal=�6   .+�5&      d�  ' #MaterialDesignHamburgerToggleButton#���  6#   �� 5'      -   5        
 ��Name$
 ConnectionStatusLbl��5'       $��Center=�6=   $��16 0 0 0q�6X   !Material Design In XAML Toolkit6�   �� 5(      -   5        $
 ServerStatus��5(       $��Center=�66   $��16 0 0 0q�6Q   Server Respon6r   5+      5,      5-       � 5=   
   -   5        $��Frame��5=   -   $��1��6   $��1��6   6<   �� 5>   
   $��1��6   ���	#FF5B5B5B$��RightToLeft=�6;   .��5?      �� -   5        $��
FriendView��5?   3   $��LeftToRight=�6   5F      5H      5K      5L      