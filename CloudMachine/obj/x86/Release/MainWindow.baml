   M S B A M L   `   `   `  ����   CloudMachine    CloudMachine.MainWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006       � Title$  
MainWindow��$	��900��$
��1440�� � WindowStartupLocation$ CenterScreen=� � WindowStyle$
 None=� � 
ResizeMode$ NoResize=� � WindowState$ 	Maximized=� � AllowsTransparency . ���	#00FFFFFF � Icon$ xinzheku.ico�� � 	Resources  �� MergedDictionaries	 �� Source��  �� $	 /Themes/ImageStyle.xaml?�%    .��� 
 � ShowGridLines
 .  
������� � RowDefinitions �� $	��80* ��� $	��40* ��� $
��620* ��� $
��150* � � ColumnDefinitions �� $��1040* ��� $
��380* �.+��� -    ��Name$ imgBackground��$��4��$��2��$��0��$��0��$
��Fill=��� -   $ AdImg��$��2��$��0��$��2��$
��Fill=�� $��1��$��1�� �� $
��180* ��� $
��200* �.+��� -    ��Name$ txtMachineCode��$��13(�$��0��
�������$
��Left=�$��Center=��� -   $ tbTime��$��13(�$��1��
�������$
��Left=�$��Center=�� $��3��$��0��$��2�� �� $
��355* ��� $
��690* ��� $
��355* �.+��� -   $ MainBottomImage��$��1��$��Center=�$��Center=�$	��110��� $��1��$��Center=� �� $	��25* ��� $
��260* ��� $	��50* ��� $	��50* ��� $
��270* ��� $	��20* �.+��� -   $ imgPrint��$��1��$��2��$��Right=�$��Center=�$��90���� -   $ imgScan��$��3��$��2��$
��Left=�$��Center=�$��90���� -   $ MainBottomTopImage��$��2��$��2��$��Center=�$��Center=�$��90��� $��2��$��Center=� �� $
��140* ��� $
��170* ��� $	��60* �.+��� -	   $ 	imgQrFram��$��1��$��Center=�$��Center=�$	��150��� $��1�� �� $��5* ��� $
��110* ��� $	��15* �.+��� -
   $ imgQr��$��1��$��Center=�$	��110��