   M S B A M L   `   `   `  ����N  JServerUIApplication, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullC&clr-namespace:ServerUIApplication.ViewServerUIApplication.View  5      M+clr-namespace:ServerUIApplication.ViewModelServerUIApplication.ViewModel  5      K*clr-namespace:ServerUIApplication.InternalServerUIApplication.Internal  5      *    $ServerUIApplication.View.MainControl   5      W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      8local&clr-namespace:ServerUIApplication.View     5      :vm+clr-namespace:ServerUIApplication.ViewModel     5      ?internal*clr-namespace:ServerUIApplication.Internal     5        � DataContext  5      8   2ServerUIApplication.ViewModel.MainControlViewModel  5   
   .��5      � ��5   
   �� 5       �� EndPoint$ 0.5,1&�6"    �� 
StartPoint$ 0.5,0&�63   .��5      �  � Color$ 	#FF151515��6    � Offset$ 1Q�61   � 5      $ 	#FF0E0E0E��6   5      .+�5   
   �  � ColumnDefinitions 5      �� 5      $
��0.3* �6#   �� 5      .+�5      �� $��10q�6   .+�5      �� $	��Run��6   $��40��6'   
���   �
���������6Y   �� *Run+�� 5      $
��Stop��6   $��40��6(   
���   �
���������6Z   �� *Stop+�� 5      $
��Exit��6   $��40��6(   
���   �
�������6G   �� 5      $��
Disconnect��6   $��40��6.   
���   �
�������6M   5      � 5       $��1��6    � RowDefinitions 5!      �� 5"      �� 5#      .+�5%      �� 
������ 2   ,ServerUIApplication.Internal.ListBoxBehavior   ScrollOnNewItem$
 True��5&      $��Hidden=�5'      ��5%      �� *Logs+��5(      �� 5)      .R�5*      �� 
���������6(   �� 69   5+      5-      �� 5.      
������ $��1��6Q   $��Hidden=�5/      $
 True��50      ��5.      �� *Users+��51      �� 52      .R�53      ��  �� Orientation$ 
Horizontal=�6)   .+�54   "   �� 
�������$��10,0,0,0q�6�   ��6,   �� *Client.RemoteEndPoint+	 �� StringFormat$	 Remote: {0}��6�   �� 55   "   
�������$��10,0,0,0q�6n   ��6,   �� *	User.Id+$	 ID: {0}��6n   �� 56   "   
�������$��10,0,0,0q�6t   ��6,   �� *
User.Login+$	 
Login: {0}��6t   �� 57   "   
�������$��10,0,0,0q�6z   ��6,   �� *User.Nickname+$	 Nickname: {0}��6z   �� 58   "   
�������$��10,0,0,0q�6v   ��6,   �� *User.Status+$	 Status: {0}��6v   59      5:      5<      5=      5>      5?      5@      