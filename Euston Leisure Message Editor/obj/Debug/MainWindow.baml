   M S B A M L   `   `   `  ����X  TEuston Leisure Message Editor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:Message_EditorMessage_Editor  5   	       Message_Editor.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .localclr-namespace:Message_Editor     5   	   -   5          �Name$  Editor��5   	    � Title$ Message Editor��5   	   $	��790��6    $	��640��6-   $
��Left=�69   $	��Top=�6T   $��16(�6l   ���	#FF0078D7 � 
ResizeMode$ CanMinimize=�6�   $��1q�6�    � Left$ 10��6�   
 � Top$	 100��6�   .��5	      � .+�5
   
   �� -   5         ��Name$ type_of_message��5
      $
��Left=�6.   $��33��6I   $��
100,10,0,0q�6U   $	��Top=�6i   $	��500��6�    �� Orientation$ 
Horizontal=�6�   .+�5      	� -   5         	�Name$	 SMS��5      $	��SMS��6'   $��20,0,0,0q�66   $��28��6^   $��Center=�6j   	� 5      -   5        $ Email��5      $��E-mail��6)   $��	130,0,0,0q�6:   $��28��6e   $��Center=�6q   	� 5      -   5        $ Tweet��5      $��Tweet��6)   $��	130,0,0,0q�69   $��28��6d   $��Center=�6p   5      �� 5   
   -   5         ��Name$ TextInput_Header��5      $
��Left=�6.   $��32��6J   $��	65,60,0,0q�6V   $	��Top=�6i   $	��535��6�   #��� ��6�   $��20(�6�   �� 5   
   $	��ID:��6   $
��Left=�6   $��30��69   $��	20,60,0,0q�6E   $	��Top=�6X   $��80��6p   $
��Bold"�6{   �� 5   
   $��Body:��6   $
��Left=�6    $��30��6;   $��
20,107,0,0q�6G   $	��Top=�6[   $��80��6s   $
��Bold"�6~   �� 5   
   $��    Sender:��6   $
��Left=�6&   $��32��6A   $��
20,148,0,0q�6M   $	��Top=�6a   $	��100��6y   $��Medium"�6�   �� 5   
   $��    Subject:��6   $
��Left=�6'   $��32��6B   $��
20,180,0,0q�6N   $	��Top=�6b   $	��100��6z   $��Medium"�6�   �� 5   
   $
��Left=�6   $��64��6/   $��
20,150,0,0q�6;   
�������$	��Top=�6c   $	��100��6{   	 K� ZIndex$	 -1000��6�   �� 5   
   -   5        
 ��Name$
 TextInput_Sender��5      $
��Left=�6,   $��32��6G   $��120,150,0,0q�6S    �� TextWrapping$
 Wrap=�6h   $	��Top=�6|   $	��482��6�    �� MaxLines$ 1��6�    �� 	MaxLength$	 254��6   ���	#FFABADB36  �� 5   
   -   5        $
 TextInput_Subject��5      $
��Left=�6-   $��32��6H   $��120,182,0,0q�6T   $
 Wrap=�6i   $	��Top=�6}   $	��482��6�   $ 20��6�   $ 1��6�    �� 	IsEnabled . 6�   �� 5   
   $ ��        Sport Centre Code:��6   $
��Left=�65   $��32��6P   $��
20,224,0,0q�6\   $	��Top=�6p   $	��190��6�   $��Medium"�6�   $	 -1��6�   �� 5   
   $!��        Nature of Incident:��6   $
��Left=�66   $��32��6Q   $��
20,256,0,0q�6]   $	��Top=�6q   $	��190��6�   $��Medium"�6�   $	 -1��6�   �� 5   
   $
��Left=�6   $��64��6/   $��
20,224,0,0q�6;   
�������$	��Top=�6c   $	��580��6{   $	 -1000��6�   �� 5   
   -	   5        $ 
 TextInput_SportCentreCode1��5      $
��Left=�66   $��32��6Q   $��210,224,0,0q�6]   $
 Wrap=�6r   $	��Top=�6�   $��32��6�   $ 2��6�   $��22(�6�   $	 -1��6�   6  �� 5   
   $
��Left=�6   $��32��6-   $��242,224,0,0q�69   $
 Wrap=�6N   $	��Top=�6b   $��28��6z   $��22(�6�   $�� -��6�   {�.  $	 -1��6�   �� 5    
   -
   5        $ 
 TextInput_SportCentreCode2��5       $
��Left=�66   $��32��6Q   $��270,224,0,0q�6]   $
 Wrap=�6r   $	��Top=�6�   $��42��6�   $ 3��6�   $��22(�6�   $	 -1��6�   6  �� 5!   
   $
��Left=�6   $��32��6-   $��312,224,0,0q�69   $
 Wrap=�6N   $	��Top=�6b   $��28��6z   $��22(�6�   $�� -��6�   {�.  $	 -1��6�   �� 5"   
   -   5        $ 
 TextInput_SportCentreCode3��5"      $
��Left=�66   $��32��6Q   $��340,224,0,0q�6]   $
 Wrap=�6r   $	��Top=�6�   $��32��6�   $ 2��6�   $��22(�6�   $	 -1��6�   6  �� 5#   
   -   5         ��Name$! ComboInput_NatureOfIncident��5#      $
��Left=�68   $��32��6S   $��210,256,0,0q�6_   $	��Top=�6t   $	��390��6�    � Cursor$
 Hand��6�    J� RenderTransformOrigin$ 0.456,0.644&�6�    �� SelectedIndex$ 0��6�   $	 -1��6�   .@�5$      �� $$��Select the nature of incident:��6   $��Medium"�6D   �� 5%      $��Theft of Properties��6   �� 5&      $��Staff Attack��6   �� 5'      $��Device Damage��6   �� 5(      $
��Raid��6   �� 5)      $��Customer Attack��6   �� 5*      $��Staff Abuse��6   �� 5+      $��Bomb Threat��6   �� 5,      $��	Terrorism��6   �� 5-      $��Suspicious Incident��6   �� 5.      $��Sport Injury��6   �� 5/      $��Personal Info Leak��6   50      �� 52   
   -   5        $
 TextInput_Message��52       �� AcceptsReturn . $
��Left=�6B   $
 Wrap=�6]   $��Bottom=�6q   $	��582��6�   $
 Hand��6�    . $	��200��6�   $	��465��6�   $
��Auto=�6�   $��	20,0,0,75q�6  6"  �� 54   
   -   5         ��Name$ ButtonInput_Save��54      $
��Save��6+   $
��Left=�6:   $��
20,698,0,0q�6U   $	��Top=�6i   $	��190��6�   $��40��6�   6�   �� 55   
   -   5        $ ButtonInput_Save_To��55      $��Save To��6.   $
��Left=�6@   $��217,698,0,0q�6[   $	��Top=�6p   $	��190��6�   $��40��6�   6�   �� 56   
   -   5        $ ButtonInput_Save_As��56      $��Save As��6.   $
��Left=�6@   $��412,698,0,0q�6[   $	��Top=�6p   $	��190��6�   $��40��6�   6�   59      5:      