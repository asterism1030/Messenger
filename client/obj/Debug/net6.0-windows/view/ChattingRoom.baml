   M S B A M L   `   `   `  ����A  =Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:Client.viewClient.view  5   	       Client.view.ChattingRoom   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:Client.view     5   	     � Title$  messenger_1��5   	   $	��510��6   $	��400��6*   .��5	      � 
������� � ColumnDefinitions 5
   
   �� 5       � RowDefinitions 5   
   �� 5      $
��390* �6   �� 5      $	��75* �6   .+�5   
   �� $��0��6   $��0��6%   $��10q�62   
������ .+�5      �� -   5         ��Name$ sv_chathistory��5      $��Disabled=�63   $
��Auto=�6\   .��5      �� -   5         ��Name$ lv_chathistory��5      $	��385��63   ��5      �� 5      .R�5      ��  �� Orientation$ Vertical=�6)   $
��Left=�6@   .+�5   "   �� $��10(�6I   $
��Wrap=�6W   $	��Top=�6k   $��
0, 0, 0, 0q�6�   ���	#FF393939��6,   �� *chatterName+6�   �� 5   "   $
��Wrap=�6E   
������ $	��300��6r   ��6,   �� *	content+6r   5      5      5      5      5      �� 5   
   $��0��6   $��1��6%   $	��Top=�62   .+�5       �� -   5         ��Name$ 
tb_message��5    X   $��55��6   $	��370��6"   $��1q�6.   ���#2196F3 �� 	MaxLength$	 300��5!       �� TextWrapping$
 Wrap=�6)   	 �� AcceptsReturn$	 False��6=   6S   5"      5#      5$      