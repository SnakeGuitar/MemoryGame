˝
6C:\MemoryGame\Client\Client\Properties\AssemblyInfo.cs
[

 
assembly

 	
:

	 

AssemblyTitle

 
(

 
$str

 !
)

! "
]

" #
[ 
assembly 	
:	 

AssemblyDescription 
( 
$str !
)! "
]" #
[ 
assembly 	
:	 
!
AssemblyConfiguration  
(  !
$str! #
)# $
]$ %
[ 
assembly 	
:	 

AssemblyCompany 
( 
$str 
) 
] 
[ 
assembly 	
:	 

AssemblyProduct 
( 
$str #
)# $
]$ %
[ 
assembly 	
:	 

AssemblyCopyright 
( 
$str 0
)0 1
]1 2
[ 
assembly 	
:	 

AssemblyTrademark 
( 
$str 
)  
]  !
[ 
assembly 	
:	 

AssemblyCulture 
( 
$str 
) 
] 
[ 
assembly 	
:	 


ComVisible 
( 
false 
) 
] 
["" 
assembly"" 	
:""	 

	ThemeInfo"" 
("" &
ResourceDictionaryLocation## 
.## 
None## #
,### $&
ResourceDictionaryLocation&& 
.&& 
SourceAssembly&& -
))) 
])) 
[33 
assembly33 	
:33	 

AssemblyVersion33 
(33 
$str33 $
)33$ %
]33% &
[44 
assembly44 	
:44	 

AssemblyFileVersion44 
(44 
$str44 (
)44( )
]44) *ƒ
5C:\MemoryGame\Client\Client\Views\TitleScreen.xaml.cs
	namespace 	
Client
 
. 
Views 
{ 
public 

partial 
class 
TitleScreen $
:% &
Window' -
{ 
public 
TitleScreen 
( 
) 
{ 	
InitializeComponent 
(  
)  !
;! "
} 	
private 
void $
ButtonLoginAsGuest_Click -
(- .
object. 4
sender5 ;
,; <
RoutedEventArgs= L
eM N
)N O
{ 	
NavigationHelper 
. 

NavigateTo '
(' (
this( ,
,, -
new. 1
EnterUsernameGuest2 D
(D E
)E F
)F G
;G H
} 	
private 
void 
ButtonLogIn_Click &
(& '
object' -
sender. 4
,4 5
RoutedEventArgs6 E
eF G
)G H
{ 	
NavigationHelper 
. 

NavigateTo '
(' (
this( ,
,, -
new. 1
Login2 7
(7 8
)8 9
)9 :
;: ;
} 	
private 
void 
ButtonSignIn_Click '
(' (
object( .
sender/ 5
,5 6
RoutedEventArgs7 F
eG H
)H I
{ 	
NavigationHelper 
. 

NavigateTo '
(' (
this( ,
,, -
new. 1
RegisterAccount2 A
(A B
)B C
)C D
;D E
} 	
private!! 
void!!  
ButtonExitGame_Click!! )
(!!) *
object!!* 0
sender!!1 7
,!!7 8
RoutedEventArgs!!9 H
e!!I J
)!!J K
{"" 	
var## 
confirmationBox## 
=##  !
new##" %"
ConfirmationMessageBox##& <
(##< =
Lang$$ 
.$$ !
Global_Title_ExitGame$$ *
,$$* +
Lang$$, 0
.$$0 1#
Global_Message_ExitGame$$1 H
,$$H I
this%% 
,%% "
ConfirmationMessageBox%% ,
.%%, -
ConfirmationBoxType%%- @
.%%@ A
Critic%%A G
)%%G H
;%%H I
if'' 
('' 
confirmationBox'' 
.''  

ShowDialog''  *
(''* +
)''+ ,
==''- /
true''0 4
)''4 5
{(( 
NavigationHelper))  
.))  !
ExitApplication))! 0
())0 1
)))1 2
;))2 3
}** 
}++ 	
},, 
}-- ó4
7C:\MemoryGame\Client\Client\Views\Session\Login.xaml.cs
	namespace 	
Client
 
. 
Views 
. 
Session 
{ 
public 

partial 
class 
Login 
:  
Window! '
{ 
public 
Login 
( 
) 
{ 	
InitializeComponent 
(  
)  !
;! "
} 	
private 
void "
TextBox_PreviewKeyDown +
(+ ,
object, 2
sender3 9
,9 :
KeyEventArgs; G
eH I
)I J
{ 	
if 
( 
e 
. 
Key 
== 
Key 
. 
Space "
)" #
{ 
e 
. 
Handled 
= 
true  
;  !
} 
} 	
private!! 
async!! 
void!! #
ButtonAcceptLogin_Click!! 2
(!!2 3
object!!3 9
sender!!: @
,!!@ A
RoutedEventArgs!!B Q
e!!R S
)!!S T
{"" 	
string## 
email## 
=## 
TextBoxEmailInput## ,
.##, -
Text##- 1
?##1 2
.##2 3
Trim##3 7
(##7 8
)##8 9
;##9 :
string$$ 
password$$ 
=$$ $
PasswordBoxPasswordInput$$ 6
.$$6 7
Password$$7 ?
;$$? @
LabelEmailError&& 
.&& 
Content&& #
=&&$ %
$str&&& (
;&&( )
LabelPasswordError'' 
.'' 
Content'' &
=''' (
$str'') +
;''+ ,
ValidationCode)) 
validationEmail)) *
=))+ ,
ValidateEmail))- :
()): ;
email)); @
)))@ A
;))A B
if** 
(** 
validationEmail** 
!=**  "
ValidationCode**# 1
.**1 2
Success**2 9
)**9 :
{++ 
LabelEmailError,, 
.,,  
Content,,  '
=,,( )
	GetString,,* 3
(,,3 4
validationEmail,,4 C
),,C D
;,,D E
return-- 
;-- 
}.. 
ValidationCode00 
validationPassword00 -
=00. /
ValidatePassword000 @
(00@ A
password00A I
)00I J
;00J K
if11 
(11 
validationPassword11 "
!=11# %
ValidationCode11& 4
.114 5
Success115 <
)11< =
{22 
LabelPasswordError33 "
.33" #
Content33# *
=33+ ,
	GetString33- 6
(336 7
validationPassword337 I
)33I J
;33J K
return44 
;44 
}55 
ButtonAcceptLogin77 
.77 
	IsEnabled77 '
=77( )
false77* /
;77/ 0
try99 
{:: 
LoginResponse;; 
response;; &
=;;' (
await;;) .
UserServiceManager;;/ A
.;;A B
Instance;;B J
.;;J K

LoginAsync;;K U
(;;U V
email;;V [
,;;[ \
password;;] e
);;e f
;;;f g
if== 
(== 
response== 
.== 
Success== $
)==$ %
{>> 
UserSession?? 
.??  
StartSession??  ,
(??, -
response??- 5
.??5 6
SessionToken??6 B
,??B C
response??D L
.??L M
User??M Q
)??Q R
;??R S
newAA 
CustomMessageBoxAA (
(AA( )
LangBB 
.BB %
Global_Title_LoginSuccessBB 6
,BB6 7
stringCC 
.CC 
FormatCC %
(CC% &
LangCC& *
.CC* +"
Global_Message_WelcomeCC+ A
,CCA B
responseCCC K
.CCK L
UserCCL P
.CCP Q
UsernameCCQ Y
)CCY Z
,CCZ [
thisDD 
,DD 
MessageBoxTypeDD ,
.DD, -
SuccessDD- 4
)DD4 5
.DD5 6

ShowDialogDD6 @
(DD@ A
)DDA B
;DDB C
NavigationHelperFF $
.FF$ %

NavigateToFF% /
(FF/ 0
thisFF0 4
,FF4 5
newFF6 9
MainMenuFF: B
(FFB C
)FFC D
)FFD E
;FFE F
}GG 
elseHH 
{II 
stringJJ 
errorMessageJJ '
=JJ( )
(JJ* +
responseJJ+ 3
.JJ3 4

MessageKeyJJ4 >
==JJ? A

ServerKeysJJB L
.JJL M
UserAlreadyLoggedInJJM `
)JJ` a
?KK 
LangKK 
.KK +
Global_Error_InvalidCredentialsKK >
:LL 
	GetStringLL #
(LL# $
responseLL$ ,
.LL, -

MessageKeyLL- 7
)LL7 8
;LL8 9
newNN 
CustomMessageBoxNN (
(NN( )
LangOO 
.OO $
Global_Title_LoginFailedOO 5
,OO5 6
errorMessageOO7 C
,OOC D
thisPP 
,PP 
MessageBoxTypePP ,
.PP, -
ErrorPP- 2
)PP2 3
.PP3 4

ShowDialogPP4 >
(PP> ?
)PP? @
;PP@ A
ButtonAcceptLoginRR %
.RR% &
	IsEnabledRR& /
=RR0 1
trueRR2 6
;RR6 7
}SS 
}TT 
catchUU 
(UU 
	ExceptionUU 
exUU 
)UU  
{VV 
ExceptionManagerWW  
.WW  !
HandleWW! '
(WW' (
exWW( *
,WW* +
thisWW, 0
,WW0 1
(WW2 3
)WW3 4
=>WW5 7
ButtonAcceptLoginWW8 I
.WWI J
	IsEnabledWWJ S
=WWT U
trueWWV Z
)WWZ [
;WW[ \
}XX 
}YY 	
private[[ 
void[[ )
ButtonBackToTitleScreen_Click[[ 2
([[2 3
object[[3 9
sender[[: @
,[[@ A
RoutedEventArgs[[B Q
e[[R S
)[[S T
{\\ 	
NavigationHelper]] 
.]] 

NavigateTo]] '
(]]' (
this]]( ,
,]], -
this]]. 2
.]]2 3
Owner]]3 8
??]]9 ;
new]]< ?
TitleScreen]]@ K
(]]K L
)]]L M
)]]M N
;]]N O
}^^ 	
}__ 
}`` –,
'C:\MemoryGame\Client\Client\App.xaml.cs
	namespace 	
Client
 
{ 
public 

partial 
class 
App 
: 
Application *
{ 
public 
App 
( 
) 
{ 	
this 
. (
DispatcherUnhandledException -
+=. 0,
 App_DispatcherUnhandledException1 Q
;Q R
TaskScheduler 
. #
UnobservedTaskException 1
+=2 41
%TaskScheduler_UnobservedTaskException5 Z
;Z [
	AppDomain 
. 
CurrentDomain #
.# $
UnhandledException$ 6
+=7 9,
 CurrentDomain_UnhandledException: Z
;Z [
} 	
	protected 
override 
void 
	OnStartup  )
() *
StartupEventArgs* :
e; <
)< =
{ 	
string 
savedLangCode  
=! "
Client# )
.) *

Properties* 4
.4 5
Settings5 =
.= >
Default> E
.E F
languageCodeF R
;R S
if 
( 
string 
. 
IsNullOrEmpty $
($ %
savedLangCode% 2
)2 3
)3 4
{   
savedLangCode!! 
=!! 
$str!!  '
;!!' (
}"" 
Client## 
.## 

Properties## 
.## 
Langs## #
.### $
Lang##$ (
.##( )
Culture##) 0
=##1 2
new##3 6
CultureInfo##7 B
(##B C
savedLangCode##C P
)##P Q
;##Q R
base%% 
.%% 
	OnStartup%% 
(%% 
e%% 
)%% 
;%% 
EventManager'' 
.''  
RegisterClassHandler'' -
(''- .
typeof''. 4
(''4 5
Window''5 ;
)''; <
,''< =
Window''> D
.''D E
LoadedEvent''E P
,''P Q
new''R U
RoutedEventHandler''V h
(''h i
OnWindowLoaded''i w
)''w x
)''x y
;''y z
}(( 	
private** 
void** 
OnWindowLoaded** #
(**# $
object**$ *
sender**+ 1
,**1 2
RoutedEventArgs**3 B
e**C D
)**D E
{++ 	
if,, 
(,, 
sender,, 
is,, 
Window,,  
window,,! '
),,' (
{-- 
window.. 
... 
Closed.. 
-=..  
OnWindowClosed..! /
;../ 0
window// 
.// 
Closed// 
+=//  
OnWindowClosed//! /
;/// 0
}00 
}11 	
private33 
void33 
OnWindowClosed33 #
(33# $
object33$ *
sender33+ 1
,331 2
	EventArgs333 <
e33= >
)33> ?
{44 	
if55 
(55 
this55 
.55 
Windows55 
.55 
Count55 "
==55# %
$num55& '
)55' (
{66 
NavigationHelper77  
.77  !
ExitApplication77! 0
(770 1
)771 2
;772 3
}88 
}99 	
private;; 
static;; 
void;; ,
 App_DispatcherUnhandledException;; <
(;;< =
object;;= C
sender;;D J
,;;J K
System;;L R
.;;R S
Windows;;S Z
.;;Z [
	Threading;;[ d
.;;d e2
%DispatcherUnhandledExceptionEventArgs	;;e ä
e
;;ã å
)
;;å ç
{<< 	
e== 
.== 
Handled== 
=== 
true== 
;== 
ExceptionManager>> 
.>> 
Handle>> #
(>># $
e>>$ %
.>>% &
	Exception>>& /
,>>/ 0
null>>1 5
,>>5 6
null>>7 ;
,>>; <
isFatal>>= D
:>>D E
false>>F K
)>>K L
;>>L M
}?? 	
privateAA 
staticAA 
voidAA 1
%TaskScheduler_UnobservedTaskExceptionAA A
(AAA B
objectAAB H
senderAAI O
,AAO P,
 UnobservedTaskExceptionEventArgsAAQ q
eAAr s
)AAs t
{BB 	
eCC 
.CC 
SetObservedCC 
(CC 
)CC 
;CC 
ExceptionManagerDD 
.DD 
HandleDD #
(DD# $
eDD$ %
.DD% &
	ExceptionDD& /
,DD/ 0
nullDD1 5
,DD5 6
nullDD7 ;
,DD; <
isFatalDD= D
:DDD E
falseDDF K
)DDK L
;DDL M
}EE 	
privateGG 
staticGG 
voidGG ,
 CurrentDomain_UnhandledExceptionGG <
(GG< =
objectGG= C
senderGGD J
,GGJ K'
UnhandledExceptionEventArgsGGL g
eGGh i
)GGi j
{HH 	
ifII 
(II 
eII 
.II 
ExceptionObjectII !
isII" $
	ExceptionII% .
exII/ 1
)II1 2
{JJ 
ExceptionManagerKK  
.KK  !
HandleKK! '
(KK' (
exKK( *
,KK* +
nullKK, 0
,KK0 1
nullKK2 6
,KK6 7
isFatalKK8 ?
:KK? @
trueKKA E
)KKE F
;KKF G
}LL 
}MM 	
}NN 
}OO »
<C:\MemoryGame\Client\Client\Views\Social\FriendsMenu.xaml.cs
	namespace 	
Client
 
. 
Views 
. 
Social 
{ 
public 

partial 
class 
FriendsMenu $
:% &
Window' -
{ 
private 
readonly 
UserServiceClient *
_proxy+ 1
;1 2
public 
FriendsMenu 
( 
) 
{ 	
InitializeComponent 
(  
)  !
;! "
_proxy 
= 
UserServiceManager '
.' (
Instance( 0
.0 1
Client1 7
;7 8
_ 
= 
LoadSocialData 
( 
)  
;  !
} 	
private 
async 
Task 
LoadSocialData )
() *
)* +
{ 	
try 
{   
var!! 
requests!! 
=!! 
await!! $
_proxy!!% +
.!!+ ,#
GetPendingRequestsAsync!!, C
(!!C D
UserSession!!D O
.!!O P
SessionToken!!P \
)!!\ ]
;!!] ^
var## 
safeRequests##  
=##! "
requests### +
??##, .
new##/ 2
FriendRequestDTO##3 C
[##C D
$num##D E
]##E F
;##F G
var%% 
requestsDisplay%% #
=%%$ %
safeRequests%%& 2
.%%2 3
Select%%3 9
(%%9 :
r%%: ;
=>%%< >
new%%? B
RequestDisplay%%C Q
{&& 
	RequestId'' 
='' 
r''  !
.''! "
	RequestId''" +
,''+ ,
SenderUsername(( "
=((# $
r((% &
.((& '
SenderUsername((' 5
,((5 6
AvatarImage)) 
=))  !
r))" #
.))# $
SenderAvatar))$ 0
!=))1 3
null))4 8
?** 
ImageHelper** %
.**% &"
ByteArrayToImageSource**& <
(**< =
r**= >
.**> ?
SenderAvatar**? K
)**K L
:++ 
null++ 
},, 
),, 
.,, 
ToList,, 
(,, 
),, 
;,, 
ListViewRequests..  
...  !
ItemsSource..! ,
=..- .
requestsDisplay../ >
;..> ?
var00 

friendsDto00 
=00  
await00! &
_proxy00' -
.00- .
GetFriendsListAsync00. A
(00A B
UserSession00B M
.00M N
SessionToken00N Z
)00Z [
;00[ \
var22 
safeFriends22 
=22  !

friendsDto22" ,
??22- /
new220 3
	FriendDTO224 =
[22= >
$num22> ?
]22? @
;22@ A
var44 
friendDisplayList44 %
=44& '
safeFriends44( 3
.443 4
Select444 :
(44: ;
f44; <
=>44= ?
new44@ C
FriendDisplay44D Q
{55 
Username66 
=66 
f66  
.66  !
Username66! )
,66) *
IsOnline77 
=77 
f77  
.77  !
IsOnline77! )
,77) *
AvatarImage88 
=88  !
f88" #
.88# $
Avatar88$ *
!=88+ -
null88. 2
?99 
ImageHelper99 %
.99% &"
ByteArrayToImageSource99& <
(99< =
f99= >
.99> ?
Avatar99? E
)99E F
::: 
null:: 
};; 
);; 
.;; 
ToList;; 
(;; 
);; 
;;; 
DataGridFriends== 
.==  
ItemsSource==  +
===, -
friendDisplayList==. ?
;==? @
}>> 
catch?? 
(?? 
	Exception?? 
ex?? 
)??  
{@@ 
ExceptionManagerAA  
.AA  !
HandleAA! '
(AA' (
exAA( *
,AA* +
thisAA, 0
)AA0 1
;AA1 2
}BB 
}CC 	
privateEE 
asyncEE 
voidEE #
ButtonSendRequest_ClickEE 2
(EE2 3
objectEE3 9
senderEE: @
,EE@ A
RoutedEventArgsEEB Q
eEER S
)EES T
{FF 	
stringGG 
usernameGG 
=GG 
TextBoxSearchUserGG /
.GG/ 0
TextGG0 4
.GG4 5
TrimGG5 9
(GG9 :
)GG: ;
;GG; <
ifHH 
(HH 
stringHH 
.HH 
IsNullOrEmptyHH $
(HH$ %
usernameHH% -
)HH- .
)HH. /
returnHH0 6
;HH6 7
ifJJ 
(JJ 
usernameJJ 
==JJ 
UserSessionJJ '
.JJ' (
UsernameJJ( 0
)JJ0 1
{KK 
newLL 
CustomMessageBoxLL $
(LL$ %
LangLL% )
.LL) *
Global_Title_ErrorLL* <
,LL< =
LangLL> B
.LLB C 
Social_Error_SelfAddLLC W
,LLW X
thisMM 
,MM 
MessageBoxTypeMM (
.MM( )
ErrorMM) .
)MM. /
.MM/ 0

ShowDialogMM0 :
(MM: ;
)MM; <
;MM< =
returnNN 
;NN 
}OO 
ButtonSendRequestQQ 
.QQ 
	IsEnabledQQ '
=QQ( )
falseQQ* /
;QQ/ 0
trySS 
{TT 
varUU 
responseUU 
=UU 
awaitUU $
_proxyUU% +
.UU+ ,"
SendFriendRequestAsyncUU, B
(UUB C
UserSessionUUC N
.UUN O
SessionTokenUUO [
,UU[ \
usernameUU] e
)UUe f
;UUf g
ifWW 
(WW 
responseWW 
.WW 
SuccessWW $
)WW$ %
{XX 
newYY 
CustomMessageBoxYY (
(YY( )
LangYY) -
.YY- . 
Global_Title_SuccessYY. B
,YYB C
stringZZ 
.ZZ 
FormatZZ %
(ZZ% &
LangZZ& *
.ZZ* +(
Friends_Label_SuccessRequestZZ+ G
,ZZG H
usernameZZI Q
)ZZQ R
,ZZR S
this[[ 
,[[ 
MessageBoxType[[ ,
.[[, -
Success[[- 4
)[[4 5
.[[5 6

ShowDialog[[6 @
([[@ A
)[[A B
;[[B C
TextBoxSearchUser]] %
.]]% &
Text]]& *
=]]+ ,
string]]- 3
.]]3 4
Empty]]4 9
;]]9 :
}^^ 
else__ 
{`` 
newaa 
CustomMessageBoxaa (
(aa( )
Langaa) -
.aa- .
Global_Title_Erroraa. @
,aa@ A
	GetStringaaB K
(aaK L
responseaaL T
.aaT U

MessageKeyaaU _
)aa_ `
,aa` a
thisbb 
,bb 
MessageBoxTypebb ,
.bb, -
Errorbb- 2
)bb2 3
.bb3 4

ShowDialogbb4 >
(bb> ?
)bb? @
;bb@ A
}cc 
}dd 
catchee 
(ee 
	Exceptionee 
exee 
)ee  
{ff 
ExceptionManagergg  
.gg  !
Handlegg! '
(gg' (
exgg( *
,gg* +
thisgg, 0
)gg0 1
;gg1 2
}hh 
finallyii 
{jj 
ButtonSendRequestkk !
.kk! "
	IsEnabledkk" +
=kk, -
truekk. 2
;kk2 3
}ll 
}mm 	
privateoo 
asyncoo 
voidoo %
ButtonAcceptRequest_Clickoo 4
(oo4 5
objectoo5 ;
senderoo< B
,ooB C
RoutedEventArgsooD S
eooT U
)ooU V
{pp 	
ifqq 
(qq 
senderqq 
isqq 
Buttonqq  
btnqq! $
&&qq% '
btnqq( +
.qq+ ,
Tagqq, /
isqq0 2
intqq3 6
	requestIdqq7 @
)qq@ A
{rr 
awaitss 
ProcessRequestss $
(ss$ %
	requestIdss% .
,ss. /
truess0 4
)ss4 5
;ss5 6
}tt 
}uu 	
privateww 
asyncww 
voidww &
ButtonDeclineRequest_Clickww 5
(ww5 6
objectww6 <
senderww= C
,wwC D
RoutedEventArgswwE T
ewwU V
)wwV W
{xx 	
ifyy 
(yy 
senderyy 
isyy 
Buttonyy  
btnyy! $
&&yy% '
btnyy( +
.yy+ ,
Tagyy, /
isyy0 2
intyy3 6
	requestIdyy7 @
)yy@ A
{zz 
await{{ 
ProcessRequest{{ $
({{$ %
	requestId{{% .
,{{. /
false{{0 5
){{5 6
;{{6 7
}|| 
}}} 	
private 
async 
Task 
ProcessRequest )
() *
int* -
	requestId. 7
,7 8
bool9 =
accept> D
)D E
{
ÄÄ 	
try
ÅÅ 
{
ÇÇ 
var
ÉÉ 
response
ÉÉ 
=
ÉÉ 
await
ÉÉ $
_proxy
ÉÉ% +
.
ÉÉ+ ,&
AnswerFriendRequestAsync
ÉÉ, D
(
ÉÉD E
UserSession
ÉÉE P
.
ÉÉP Q
SessionToken
ÉÉQ ]
,
ÉÉ] ^
	requestId
ÉÉ_ h
,
ÉÉh i
accept
ÉÉj p
)
ÉÉp q
;
ÉÉq r
if
ÖÖ 
(
ÖÖ 
response
ÖÖ 
.
ÖÖ 
Success
ÖÖ $
)
ÖÖ$ %
{
ÜÜ 
_
áá 
=
áá 
LoadSocialData
áá &
(
áá& '
)
áá' (
;
áá( )
}
àà 
else
ââ 
{
ää 
new
ãã 
CustomMessageBox
ãã (
(
ãã( )
Lang
ãã) -
.
ãã- . 
Global_Title_Error
ãã. @
,
ãã@ A
	GetString
ããB K
(
ããK L
response
ããL T
.
ããT U

MessageKey
ããU _
)
ãã_ `
,
ãã` a
this
åå 
,
åå 
MessageBoxType
åå ,
.
åå, -
Error
åå- 2
)
åå2 3
.
åå3 4

ShowDialog
åå4 >
(
åå> ?
)
åå? @
;
åå@ A
}
çç 
}
éé 
catch
èè 
(
èè 
	Exception
èè 
ex
èè 
)
èè  
{
êê 
ExceptionManager
ëë  
.
ëë  !
Handle
ëë! '
(
ëë' (
ex
ëë( *
,
ëë* +
this
ëë, 0
)
ëë0 1
;
ëë1 2
}
íí 
}
ìì 	
private
ïï 
async
ïï 
void
ïï &
ButtonRemoveFriend_Click
ïï 3
(
ïï3 4
object
ïï4 :
sender
ïï; A
,
ïïA B
RoutedEventArgs
ïïC R
e
ïïS T
)
ïïT U
{
ññ 	
if
óó 
(
óó 
sender
óó 
is
óó 
Button
óó  
btn
óó! $
&&
óó% '
btn
óó( +
.
óó+ ,
Tag
óó, /
is
óó0 2
string
óó3 9
username
óó: B
)
óóB C
{
òò 
var
ôô 
confirmationBox
ôô #
=
ôô$ %
new
ôô& )$
ConfirmationMessageBox
ôô* @
(
ôô@ A
string
öö 
.
öö 
Format
öö !
(
öö! "
Lang
öö" &
.
öö& '*
Friends_Message_RemoveFriend
öö' C
,
ööC D
username
ööE M
)
ööM N
,
ööN O
Lang
õõ 
.
õõ "
Global_Label_Confirm
õõ -
,
õõ- .
this
õõ/ 3
,
õõ3 4!
ConfirmationBoxType
õõ5 H
.
õõH I
Warning
õõI P
)
õõP Q
;
õõQ R
if
ùù 
(
ùù 
confirmationBox
ùù #
.
ùù# $

ShowDialog
ùù$ .
(
ùù. /
)
ùù/ 0
==
ùù1 3
true
ùù4 8
)
ùù8 9
{
ûû 
try
üü 
{
†† 
var
°° 
response
°° $
=
°°% &
await
°°' ,
_proxy
°°- 3
.
°°3 4
RemoveFriendAsync
°°4 E
(
°°E F
UserSession
°°F Q
.
°°Q R
SessionToken
°°R ^
,
°°^ _
username
°°` h
)
°°h i
;
°°i j
if
££ 
(
££ 
response
££ $
.
££$ %
Success
££% ,
)
££, -
{
§§ 
_
•• 
=
•• 
LoadSocialData
••  .
(
••. /
)
••/ 0
;
••0 1
}
¶¶ 
else
ßß 
{
®® 
new
©© 
CustomMessageBox
©©  0
(
©©0 1
Lang
©©1 5
.
©©5 6 
Global_Title_Error
©©6 H
,
©©H I
Lang
©©J N
.
©©N O(
Friends_Error_RemoveFriend
©©O i
,
©©i j
this
™™  $
,
™™$ %
MessageBoxType
™™& 4
.
™™4 5
Error
™™5 :
)
™™: ;
.
™™; <

ShowDialog
™™< F
(
™™F G
)
™™G H
;
™™H I
}
´´ 
}
¨¨ 
catch
≠≠ 
(
≠≠ 
	Exception
≠≠ $
ex
≠≠% '
)
≠≠' (
{
ÆÆ 
ExceptionManager
ØØ (
.
ØØ( )
Handle
ØØ) /
(
ØØ/ 0
ex
ØØ0 2
,
ØØ2 3
this
ØØ4 8
)
ØØ8 9
;
ØØ9 :
}
∞∞ 
}
±± 
}
≤≤ 
}
≥≥ 	
private
µµ 
void
µµ 
ButtonBack_Click
µµ %
(
µµ% &
object
µµ& ,
sender
µµ- 3
,
µµ3 4
RoutedEventArgs
µµ5 D
e
µµE F
)
µµF G
{
∂∂ 	
NavigationHelper
∑∑ 
.
∑∑ 

NavigateTo
∑∑ '
(
∑∑' (
this
∑∑( ,
,
∑∑, -
this
∑∑. 2
.
∑∑2 3
Owner
∑∑3 8
??
∑∑9 ;
new
∑∑< ?
MainMenu
∑∑@ H
(
∑∑H I
)
∑∑I J
)
∑∑J K
;
∑∑K L
}
∏∏ 	
public
∫∫ 
class
∫∫ 
FriendDisplay
∫∫ "
{
ªª 	
public
ºº 
string
ºº 
Username
ºº "
{
ºº# $
get
ºº% (
;
ºº( )
set
ºº* -
;
ºº- .
}
ºº/ 0
public
ΩΩ 
bool
ΩΩ 
IsOnline
ΩΩ  
{
ΩΩ! "
get
ΩΩ# &
;
ΩΩ& '
set
ΩΩ( +
;
ΩΩ+ ,
}
ΩΩ- .
public
ææ 
ImageSource
ææ 
AvatarImage
ææ *
{
ææ+ ,
get
ææ- 0
;
ææ0 1
set
ææ2 5
;
ææ5 6
}
ææ7 8
public
¿¿ 
string
¿¿ 

StatusText
¿¿ $
=>
¿¿% '
IsOnline
¿¿( 0
?
¿¿1 2
Lang
¿¿3 7
.
¿¿7 8!
Global_Label_Online
¿¿8 K
:
¿¿L M
Lang
¿¿N R
.
¿¿R S"
Global_Label_Offline
¿¿S g
;
¿¿g h
public
¡¡ 
Brush
¡¡ 
StatusColor
¡¡ $
=>
¡¡% '
IsOnline
¡¡( 0
?
¡¡1 2
Brushes
¡¡3 :
.
¡¡: ;
Green
¡¡; @
:
¡¡A B
Brushes
¡¡C J
.
¡¡J K
Gray
¡¡K O
;
¡¡O P
}
¬¬ 	
public
√√ 
class
√√ 
RequestDisplay
√√ #
{
ƒƒ 	
public
≈≈ 
int
≈≈ 
	RequestId
≈≈  
{
≈≈! "
get
≈≈# &
;
≈≈& '
set
≈≈( +
;
≈≈+ ,
}
≈≈- .
public
∆∆ 
string
∆∆ 
SenderUsername
∆∆ (
{
∆∆) *
get
∆∆+ .
;
∆∆. /
set
∆∆0 3
;
∆∆3 4
}
∆∆5 6
public
«« 
ImageSource
«« 
AvatarImage
«« *
{
««+ ,
get
««- 0
;
««0 1
set
««2 5
;
««5 6
}
««7 8
}
»» 	
}
…… 
}   ˙<
>C:\MemoryGame\Client\Client\Views\Session\SetupProfile.xaml.cs
	namespace 	
Client
 
. 
Views 
. 
Session 
{ 
public 

partial 
class 
SetupProfile %
:& '
Window( .
{ 
private 
readonly 
string 
_email  &
;& '
private 
byte 
[ 
] 
profileImage #
;# $
public 
SetupProfile 
( 
string "
email# (
)( )
{ 	
InitializeComponent 
(  
)  !
;! "
_email 
= 
email 
?? 
throw #
new$ '!
ArgumentNullException( =
(= >
nameof> D
(D E
emailE J
)J K
)K L
;L M

LabelEmail 
. 
Content 
=  
string! '
.' (
Format( .
(. /
Lang/ 3
.3 4(
Global_Label_RegisteredEmail4 P
,P Q
emailR W
)W X
;X Y
} 	
private 
void $
ButtonSelectAvatar_Click -
(- .
object. 4
sender5 ;
,; <
RoutedEventArgs= L
eM N
)N O
{   	
var!! 
avatarDialog!! 
=!! 
NavigationHelper!! /
.!!/ 0
GetOpenFileDialog!!0 A
(!!A B
Lang"" 
."" %
SetupProfile_Dialog_Title"" .
,"". /
Lang## 
.## &
SetupProfile_Dialog_Filter## /
,##/ 0
false$$ 
)$$ 
;$$ 
if&& 
(&& 
avatarDialog&& 
.&& 

ShowDialog&& '
(&&' (
)&&( )
==&&* ,
true&&- 1
)&&1 2
{'' 
try(( 
{)) 
byte** 
[** 
]** 
originalBytes** (
=**) *
File**+ /
.**/ 0
ReadAllBytes**0 <
(**< =
avatarDialog**= I
.**I J
FileName**J R
)**R S
;**S T
profileImage++  
=++! "
ImageHelper++# .
.++. /
ResizeImage++/ :
(++: ;
originalBytes++; H
,++H I
$num++J M
,++M N
$num++O R
)++R S
;++S T
ProfilePicture,, "
.,," #
Source,,# )
=,,* +
ImageHelper,,, 7
.,,7 8"
ByteArrayToImageSource,,8 N
(,,N O
profileImage,,O [
),,[ \
;,,\ ]
}-- 
catch.. 
(.. 
	Exception..  
ex..! #
)..# $
{// 
ExceptionManager00 $
.00$ %
Handle00% +
(00+ ,
ex00, .
,00. /
this000 4
)004 5
;005 6
profileImage11  
=11! "
null11# '
;11' (
ProfilePicture22 "
.22" #
Source22# )
=22* +
null22, 0
;220 1
}33 
}44 
}55 	
private77 
async77 
void77 #
ButtonAcceptSetup_Click77 2
(772 3
object773 9
sender77: @
,77@ A
RoutedEventArgs77B Q
e77R S
)77S T
{88 	
string99 
username99 
=99 
TextBoxUsername99 -
.99- .
Text99. 2
.992 3
Trim993 7
(997 8
)998 9
;999 :
LabelUsernameError:: 
.:: 
Content:: &
=::' (
$str::) +
;::+ ,
ValidationCode<< 
validationCode<< )
=<<* +
ValidateUsername<<, <
(<<< =
username<<= E
)<<E F
;<<F G
if== 
(== 
validationCode== 
!=== !
ValidationCode==" 0
.==0 1
Success==1 8
)==8 9
{>> 
LabelUsernameError?? "
.??" #
Content??# *
=??+ ,
	GetString??- 6
(??6 7
validationCode??7 E
)??E F
;??F G
return@@ 
;@@ 
}AA $
ButtonAcceptSetupProfileCC $
.CC$ %
	IsEnabledCC% .
=CC/ 0
falseCC1 6
;CC6 7
tryEE 
{FF 
LoginResponseGG 
responseGG &
=GG' (
awaitGG) .
UserServiceManagerGG/ A
.GGA B
InstanceGGB J
.GGJ K
ClientGGK Q
.GGQ R%
FinalizeRegistrationAsyncGGR k
(GGk l
_emailHH 
,HH 
usernameII 
,II 
profileImageJJ  
)JJ  !
;JJ! "
ifLL 
(LL 
responseLL 
.LL 
SuccessLL $
)LL$ %
{MM 
UserSessionNN 
.NN  
StartSessionNN  ,
(NN, -
responseNN- 5
.NN5 6
SessionTokenNN6 B
,NNB C
responseNND L
.NNL M
UserNNM Q
)NNQ R
;NNR S
newPP 
CustomMessageBoxPP (
(PP( )
LangQQ 
.QQ  
Global_Title_SuccessQQ 1
,QQ1 2
LangQQ3 7
.QQ7 8(
SetupProfile_Message_SuccessQQ8 T
,QQT U
thisRR 
,RR 
MessageBoxTypeRR ,
.RR, -
InformationRR- 8
)RR8 9
.RR9 :

ShowDialogRR: D
(RRD E
)RRE F
;RRF G
NavigationHelperTT $
.TT$ %

NavigateToTT% /
(TT/ 0
thisTT0 4
,TT4 5
newTT6 9
MainMenuTT: B
(TTB C
)TTC D
)TTD E
;TTE F
}UU 
elseVV 
{WW 
stringXX 
errorMessageXX '
=XX( )
	GetStringXX* 3
(XX3 4
responseXX4 <
.XX< =

MessageKeyXX= G
)XXG H
;XXH I
newYY 
CustomMessageBoxYY (
(YY( )
LangZZ 
.ZZ 
Global_Title_ErrorZZ /
,ZZ/ 0
errorMessageZZ1 =
,ZZ= >
this[[ 
,[[ 
MessageBoxType[[ ,
.[[, -
Error[[- 2
)[[2 3
.[[3 4

ShowDialog[[4 >
([[> ?
)[[? @
;[[@ A$
ButtonAcceptSetupProfile]] ,
.]], -
	IsEnabled]]- 6
=]]7 8
true]]9 =
;]]= >
}^^ 
}__ 
catch`` 
(`` 
	Exception`` 
ex`` 
)``  
{aa 
ExceptionManagerbb  
.bb  !
Handlebb! '
(bb' (
exbb( *
,bb* +
thisbb, 0
,bb0 1
(bb2 3
)bb3 4
=>bb5 7$
ButtonAcceptSetupProfilebb8 P
.bbP Q
	IsEnabledbbQ Z
=bb[ \
truebb] a
)bba b
;bbb c
}cc 
}dd 	
privateff 
voidff )
ButtonBackToTitleScreen_Clickff 2
(ff2 3
objectff3 9
senderff: @
,ff@ A
RoutedEventArgsffB Q
effR S
)ffS T
{gg 	
NavigationHelperhh 
.hh 

NavigateTohh '
(hh' (
thishh( ,
,hh, -
thishh. 2
.hh2 3
Ownerhh3 8
??hh9 ;
newhh< ?
TitleScreenhh@ K
(hhK L
)hhL M
)hhM N
;hhN O
}ii 	
}jj 
}kk ¬
GC:\MemoryGame\Client\Client\Views\Singleplayer\SelectDifficulty.xaml.cs
	namespace 	
Client
 
. 
Views 
. 
Singleplayer #
{ 
public

 

partial

 
class

 
SelectDifficulty

 )
:

* +
Window

, 2
{ 
public 
SelectDifficulty 
(  
)  !
{ 	
InitializeComponent 
(  
)  !
;! "
} 	
private 
void &
ButtonEasyDifficulty_Click /
(/ 0
object0 6
sender7 =
,= >
RoutedEventArgs? N
eO P
)P Q
{ 	
NavigationHelper 
. 

NavigateTo '
(' (
this( ,
,, -
new. 1 
PlayGameSingleplayer2 F
(F G
DifficultyPresetsG X
.X Y
EasyY ]
)] ^
)^ _
;_ `
} 	
private 
void (
ButtonNormalDifficulty_Click 1
(1 2
object2 8
sender9 ?
,? @
RoutedEventArgsA P
eQ R
)R S
{ 	
NavigationHelper 
. 

NavigateTo '
(' (
this( ,
,, -
new. 1 
PlayGameSingleplayer2 F
(F G
DifficultyPresetsG X
.X Y
NormalY _
)_ `
)` a
;a b
} 	
private 
void &
ButtonHardDifficulty_Click /
(/ 0
object0 6
sender7 =
,= >
RoutedEventArgs? N
eO P
)P Q
{ 	
NavigationHelper 
. 

NavigateTo '
(' (
this( ,
,, -
new. 1 
PlayGameSingleplayer2 F
(F G
DifficultyPresetsG X
.X Y
HardY ]
)] ^
)^ _
;_ `
} 	
private   
void   +
ButtonCustomizeDifficulty_Click   4
(  4 5
object  5 ;
sender  < B
,  B C
RoutedEventArgs  D S
e  T U
)  U V
{!! 	
NavigationHelper"" 
."" 

NavigateTo"" '
(""' (
this""( ,
,"", -
new"". 1
CustomizeGame""2 ?
(""? @
)""@ A
)""A B
;""B C
}## 	
private%% 
void%% &
ButtonBackToMainMenu_Click%% /
(%%/ 0
object%%0 6
sender%%7 =
,%%= >
RoutedEventArgs%%? N
e%%O P
)%%P Q
{&& 	
NavigationHelper'' 
.'' 

NavigateTo'' '
(''' (
this''( ,
,'', -
this''. 2
.''2 3
Owner''3 8
??''9 ;
new''< ?
MainMenu''@ H
(''H I
)''I J
)''J K
;''K L
}(( 	
})) 
}** ˆW
KC:\MemoryGame\Client\Client\Views\Singleplayer\PlayGameSingleplayer.xaml.cs
	namespace 	
Client
 
. 
Views 
. 
Singleplayer #
{ 
public 

partial 
class  
PlayGameSingleplayer -
:. /
Window0 6
{ 
public  
ObservableCollection #
<# $
Card$ (
>( )
Cards* /
{0 1
get2 5
;5 6
set7 :
;: ;
}< =
public 
int 
GameRows 
{ 
get !
;! "
set# &
;& '
}( )
public 
int 
GameColumns 
{  
get! $
;$ %
set& )
;) *
}+ ,
private 
readonly 
GameManager $
_gameManager% 1
;1 2
public  
PlayGameSingleplayer #
(# $
GameConfiguration$ 5
config6 <
)< =
{ 	
InitializeComponent 
(  
)  !
;! "
if!! 
(!! 
config!! 
.!! 

NumberRows!! !
*!!" #
config!!$ *
.!!* +
NumberColumns!!+ 8
!=!!9 ;
config!!< B
.!!B C
NumberOfCards!!C P
)!!P Q
{"" 
config## 
.## 

NumberRows## !
=##" #
$num##$ %
;##% &
config$$ 
.$$ 
NumberColumns$$ $
=$$% &
$num$$' (
;$$( )
config%% 
.%% 
NumberOfCards%% $
=%%% &
$num%%' )
;%%) *
}&& 
Cards(( 
=(( 
new((  
ObservableCollection(( ,
<((, -
Card((- 1
>((1 2
(((2 3
)((3 4
;((4 5
	GameBoard)) 
.)) 
ItemsSource)) !
=))" #
Cards))$ )
;))) *
GameRows** 
=** 
config** 
.** 

NumberRows** (
;**( )
GameColumns++ 
=++ 
config++  
.++  !
NumberColumns++! .
;++. /
this-- 
.-- 
DataContext-- 
=-- 
this-- #
;--# $
_gameManager// 
=// 
new// 
GameManager// *
(//* +
Cards//+ 0
)//0 1
;//1 2
ConfigureGameEvents00 
(00  
)00  !
;00! "
try22 
{33 
_gameManager44 
.44 !
StartSingleplayerGame44 2
(442 3
config443 9
)449 :
;44: ;
}55 
catch66 
(66 
	Exception66 
ex66 
)66  
{77 
ExceptionManager88  
.88  !
Handle88! '
(88' (
ex88( *
,88* +
this88, 0
,880 1
(882 3
)883 4
=>885 7
this888 <
.88< =
Close88= B
(88B C
)88C D
)88D E
;88E F
}99 
}:: 	
private>> 
void>> 
ConfigureGameEvents>> (
(>>( )
)>>) *
{?? 	
_gameManager@@ 
.@@ 
TimerUpdated@@ %
+=@@& (
OnTimerUpdated@@) 7
;@@7 8
_gameManagerAA 
.AA 
ScoreUpdatedAA %
+=AA& (
OnScoreUpdatedAA) 7
;AA7 8
_gameManagerBB 
.BB 
GameWonBB  
+=BB! #
	OnGameWonBB$ -
;BB- .
_gameManagerCC 
.CC 
GameLostCC !
+=CC" $

OnGameLostCC% /
;CC/ 0
}DD 	
privateFF 
voidFF 
UnsubscribeEventsFF &
(FF& '
)FF' (
{GG 	
_gameManagerHH 
.HH 
TimerUpdatedHH %
-=HH& (
OnTimerUpdatedHH) 7
;HH7 8
_gameManagerII 
.II 
ScoreUpdatedII %
-=II& (
OnScoreUpdatedII) 7
;II7 8
_gameManagerJJ 
.JJ 
GameWonJJ  
-=JJ! #
	OnGameWonJJ$ -
;JJ- .
_gameManagerKK 
.KK 
GameLostKK !
-=KK" $

OnGameLostKK% /
;KK/ 0
}LL 	
privateRR 
voidRR 
OnTimerUpdatedRR #
(RR# $
stringRR$ *

timeStringRR+ 5
)RR5 6
{SS 	

LabelTimerTT 
.TT 
ContentTT 
=TT  

timeStringTT! +
;TT+ ,
}UU 	
privateWW 
voidWW 
OnScoreUpdatedWW #
(WW# $
intWW$ '
newScoreWW( 0
)WW0 1
{XX 	

LabelScoreYY 
.YY 
ContentYY 
=YY  
newScoreYY! )
.YY) *
ToStringYY* 2
(YY2 3
)YY3 4
;YY4 5
}ZZ 	
private\\ 
void\\ 
	OnGameWon\\ 
(\\ 
)\\  
{]] 	
string^^ 

winnerName^^ 
=^^ 
UserSession^^  +
.^^+ ,
Username^^, 4
??^^5 7
$str^^8 @
;^^@ A
string__ 
	statsInfo__ 
=__ 
$"__ !
{__! "
Lang__" &
.__& '
Global_Label_Score__' 9
}__9 :
$str__: ;
{__; <

LabelScore__< F
.__F G
Content__G N
}__N O
$str__O R
{__R S
Lang__S W
.__W X,
 MatchSummary_Label_TimeRemaining__X x
}__x y
$str__y z
{__z {

LabelTimer	__{ Ö
.
__Ö Ü
Content
__Ü ç
}
__ç é
"
__é è
;
__è ê
ShowMatchSummaryaa 
(aa 

winnerNameaa '
,aa' (
	statsInfoaa) 2
)aa2 3
;aa3 4
}bb 	
privatedd 
voiddd 

OnGameLostdd 
(dd  
)dd  !
{ee 	
stringff 
titleff 
=ff 
Langff 
.ff  '
Singleplayer_Title_TimeOverff  ;
;ff; <
stringgg 
	statsInfogg 
=gg 
$"gg !
{gg! "
Langgg" &
.gg& '
Global_Label_Scoregg' 9
}gg9 :
$strgg: <
{gg< =

LabelScoregg= G
.ggG H
ContentggH O
}ggO P
"ggP Q
;ggQ R
ShowMatchSummaryii 
(ii 
titleii "
,ii" #
	statsInfoii$ -
)ii- .
;ii. /
}jj 	
privatell 
voidll 
ShowMatchSummaryll %
(ll% &
stringll& ,
titlell- 2
,ll2 3
stringll4 :
statsll; @
)ll@ A
{mm 	
varnn 
summaryWindownn 
=nn 
newnn  #
MatchSummarynn$ 0
(nn0 1
titlenn1 6
,nn6 7
statsnn8 =
)nn= >
;nn> ?
NavigationHelperoo 
.oo 

ShowDialogoo '
(oo' (
thisoo( ,
,oo, -
summaryWindowoo. ;
)oo; <
;oo< =.
"ButtonBackToSelectDifficulty_Clickpp .
(pp. /
nullpp/ 3
,pp3 4
nullpp5 9
)pp9 :
;pp: ;
}qq 	
privateww 
asyncww 
voidww 

Card_Clickww %
(ww% &
objectww& ,
senderww- 3
,ww3 4
RoutedEventArgsww5 D
ewwE F
)wwF G
{xx 	
ifyy 
(yy 
senderyy 
isyy 
Buttonyy  
buttonyy! '
&&yy( *
buttonyy+ 1
.yy1 2
DataContextyy2 =
isyy> @
CardyyA E
clickedCardyyF Q
)yyQ R
{zz 
try{{ 
{|| 
await}} 
_gameManager}} &
.}}& '
HandleCardClick}}' 6
(}}6 7
clickedCard}}7 B
)}}B C
;}}C D
}~~ 
catch 
( 
	Exception  
ex! #
)# $
{
ÄÄ 
ExceptionManager
ÅÅ $
.
ÅÅ$ %
Handle
ÅÅ% +
(
ÅÅ+ ,
ex
ÅÅ, .
,
ÅÅ. /
this
ÅÅ0 4
)
ÅÅ4 5
;
ÅÅ5 6
}
ÇÇ 
}
ÉÉ 
}
ÑÑ 	
private
ÜÜ 
void
ÜÜ "
ButtonSettings_Click
ÜÜ )
(
ÜÜ) *
object
ÜÜ* 0
sender
ÜÜ1 7
,
ÜÜ7 8
RoutedEventArgs
ÜÜ9 H
e
ÜÜI J
)
ÜÜJ K
{
áá 	
_gameManager
àà 
.
àà 
StopGame
àà !
(
àà! "
)
àà" #
;
àà# $
var
ââ 
settingsWindow
ââ 
=
ââ  
new
ââ! $
Settings
ââ% -
(
ââ- .
)
ââ. /
;
ââ/ 0
NavigationHelper
ää 
.
ää 

ShowDialog
ää '
(
ää' (
this
ää( ,
,
ää, -
settingsWindow
ää. <
)
ää< =
;
ää= >
}
ãã 	
public
çç 
void
çç 0
"ButtonBackToSelectDifficulty_Click
çç 6
(
çç6 7
object
çç7 =
sender
çç> D
,
ççD E
RoutedEventArgs
ççF U
e
ççV W
)
ççW X
{
éé 	
UnsubscribeEvents
èè 
(
èè 
)
èè 
;
èè  
_gameManager
êê 
.
êê 
StopGame
êê !
(
êê! "
)
êê" #
;
êê# $
NavigationHelper
ëë 
.
ëë 

NavigateTo
ëë '
(
ëë' (
this
ëë( ,
,
ëë, -
this
ëë. 2
.
ëë2 3
Owner
ëë3 8
??
ëë9 ;
new
ëë< ?
SelectDifficulty
ëë@ P
(
ëëP Q
)
ëëQ R
)
ëëR S
;
ëëS T
}
íí 	
	protected
îî 
override
îî 
void
îî 
OnClosed
îî  (
(
îî( )
	EventArgs
îî) 2
e
îî3 4
)
îî4 5
{
ïï 	
UnsubscribeEvents
ññ 
(
ññ 
)
ññ 
;
ññ  
_gameManager
óó 
.
óó 
StopGame
óó !
(
óó! "
)
óó" #
;
óó# $
if
òò 
(
òò 
this
òò 
.
òò 
Owner
òò 
!=
òò 
null
òò "
&&
òò# %
Application
òò& 1
.
òò1 2
Current
òò2 9
.
òò9 :

MainWindow
òò: D
!=
òòE G
this
òòH L
.
òòL M
Owner
òòM R
)
òòR S
{
ôô 
this
öö 
.
öö 
Owner
öö 
.
öö 
Show
öö 
(
öö  
)
öö  !
;
öö! "
}
õõ 
base
ùù 
.
ùù 
OnClosed
ùù 
(
ùù 
e
ùù 
)
ùù 
;
ùù 
}
ûû 	
}
°° 
}¢¢ £'
DC:\MemoryGame\Client\Client\Views\Singleplayer\CustomizeGame.xaml.cs
	namespace		 	
Client		
 
.		 
Views		 
.		 
Singleplayer		 #
{

 
public 

partial 
class 
CustomizeGame &
:' (
Window) /
{ 
public 
CustomizeGame 
( 
) 
{ 	
InitializeComponent 
(  
)  !
;! "%
ComboBoxSelectNumberCards %
.% &
SelectedIndex& 3
=4 5
$num6 7
;7 8
} 	
private 
void $
TimerSlider_ValueChanged -
(- .
object. 4
sender5 ;
,; <*
RoutedPropertyChangedEventArgs= [
<[ \
double\ b
>b c
ed e
)e f
{ 	
if 
( 
LabelTimerValue 
!=  "
null# '
)' (
{ 
LabelTimerValue 
.  
Content  '
=( )
e* +
.+ ,
NewValue, 4
.4 5
ToString5 =
(= >
$str> B
)B C
;C D
} 
} 	
private 
void  
ButtonPlayGame_Click )
() *
object* 0
sender1 7
,7 8
RoutedEventArgs9 H
eI J
)J K
{ 	
if   
(   
sender   
is   
Button    
button  ! '
)  ' (
{!! 
button"" 
."" 
	IsEnabled""  
=""! "
false""# (
;""( )
}## 
try%% 
{&& 
int'' 
selectedCards'' !
=''" #
$num''$ &
;''& '
if)) 
()) %
ComboBoxSelectNumberCards)) -
.))- .
SelectedItem)). :
is)); =
ComboBoxItem))> J
selectedItem))K W
&&))X Z
int** 
.** 
TryParse**  
(**  !
selectedItem**! -
.**- .
Content**. 5
.**5 6
ToString**6 >
(**> ?
)**? @
,**@ A
out**B E
int**F I
result**J P
)**P Q
)**Q R
{++ 
selectedCards,, !
=,," #
result,,$ *
;,,* +
}-- 
int// 
selectedTime//  
=//! "
(//# $
int//$ '
)//' (
TimerSlider//( 3
.//3 4
Value//4 9
;//9 :
var00 
(00 
Rows00 
,00 
Columns00 "
)00" #
=00$ %
DifficultyPresets00& 7
.007 8
CalculateLayout008 G
(00G H
selectedCards00H U
)00U V
;00V W
var22 
customConfig22  
=22! "
new22# &
GameConfiguration22' 8
{33 
NumberOfCards44 !
=44" #
selectedCards44$ 1
,441 2
TimeLimitSeconds55 $
=55% &
selectedTime55' 3
,553 4
DifficultyLevel66 #
=66$ %
Lang66& *
.66* + 
Global_Button_Custom66+ ?
,66? @

NumberRows77 
=77  
Rows77! %
,77% &
NumberColumns88 !
=88" #
Columns88$ +
}99 
;99 
var;; 

gameWindow;; 
=;;  
new;;! $ 
PlayGameSingleplayer;;% 9
(;;9 :
customConfig;;: F
);;F G
;;;G H
NavigationHelper<<  
.<<  !

NavigateTo<<! +
(<<+ ,
this<<, 0
,<<0 1

gameWindow<<2 <
)<<< =
;<<= >
}== 
catch>> 
(>> 
	Exception>> 
ex>> 
)>>  
{?? 
if@@ 
(@@ 
sender@@ 
is@@ 
Button@@ $
buttonError@@% 0
)@@0 1
{AA 
buttonErrorBB 
.BB  
	IsEnabledBB  )
=BB* +
trueBB, 0
;BB0 1
ExceptionManagerCC $
.CC$ %
HandleCC% +
(CC+ ,
exCC, .
,CC. /
thisCC0 4
)CC4 5
;CC5 6
}DD 
}EE 
}FF 	
privateHH 
voidHH &
ButtonBackToMainMenu_ClickHH /
(HH/ 0
objectHH0 6
senderHH7 =
,HH= >
RoutedEventArgsHH? N
eHHO P
)HHP Q
{II 	
NavigationHelperJJ 
.JJ 

NavigateToJJ '
(JJ' (
thisJJ( ,
,JJ, -
thisJJ. 2
.JJ2 3
OwnerJJ3 8
??JJ9 ;
newJJ< ?
SelectDifficultyJJ@ P
(JJP Q
)JJQ R
)JJR S
;JJS T
}KK 	
}LL 
}MM Í4
2C:\MemoryGame\Client\Client\Views\Settings.xaml.cs
	namespace

 	
Client


 
.

 
Views

 
{ 
public 

partial 
class 
Settings !
:" #
Window$ *
{ 
private 
bool 
	_isLoaded 
=  
false! &
;& '
private 
bool 
_languageChanged %
=& '
false( -
;- .
public 
Settings 
( 
bool 
changeHappened +
=, -
false. 3
)3 4
{ 	
InitializeComponent 
(  
)  !
;! "
_languageChanged 
= 
changeHappened -
;- .
LoadLanguages 
( 
) 
; 
} 	
private 
void 
LoadLanguages "
(" #
)# $
{ 	
var 
	languages 
= 
new 
List  $
<$ %
LanguageOption% 3
>3 4
{ 
new 
LanguageOption "
{# $
DisplayCultureName% 7
=8 9
$str: C
,C D
CultureCodeE P
=Q R
$strS Z
}[ \
,\ ]
new   
LanguageOption   "
{  # $
DisplayCultureName  % 7
=  8 9
$str  : C
,  C D
CultureCode  E P
=  Q R
$str  S Z
}  [ \
,  \ ]
new!! 
LanguageOption!! "
{!!# $
DisplayCultureName!!% 7
=!!8 9
$str!!: ?
,!!? @
CultureCode!!A L
=!!M N
$str!!O V
}!!W X
,!!X Y
new"" 
LanguageOption"" "
{""# $
DisplayCultureName""% 7
=""8 9
$str"": >
,""> ?
CultureCode""@ K
=""L M
$str""N U
}""V W
,""W X
new## 
LanguageOption## "
{### $
DisplayCultureName##% 7
=##8 9
$str##: ?
,##? @
CultureCode##A L
=##M N
$str##O V
}##W X
}$$ 
;$$ 
ComboBoxLanguage&& 
.&& 
ItemsSource&& (
=&&) *
	languages&&+ 4
;&&4 5
string'' 
currentLangCode'' "
=''# $
Lang''% )
.'') *
Culture''* 1
.''1 2
Name''2 6
;''6 7
var(( 
selectedLanguage((  
=((! "
	languages((# ,
.((, -
FirstOrDefault((- ;
(((; <
lang((< @
=>((A C
lang((D H
.((H I
CultureCode((I T
==((U W
currentLangCode((X g
)((g h
;((h i
if** 
(** 
selectedLanguage**  
!=**! #
null**$ (
)**( )
{++ 
ComboBoxLanguage,,  
.,,  !
SelectedItem,,! -
=,,. /
selectedLanguage,,0 @
;,,@ A
}-- 
else.. 
{// 
ComboBoxLanguage00  
.00  !
SelectedIndex00! .
=00/ 0
$num001 2
;002 3
}11 
	_isLoaded22 
=22 
true22 
;22 
}33 	
private55 
void55 -
!ComboBoxLanguage_SelectionChanged55 6
(556 7
object557 =
sender55> D
,55D E%
SelectionChangedEventArgs55F _
e55` a
)55a b
{66 	
if77 
(77 
!77 
	_isLoaded77 
)77 
return77 "
;77" #
if99 
(99 
ComboBoxLanguage99  
.99  !
SelectedItem99! -
is99. 0
LanguageOption991 ?
selectedOption99@ N
)99N O
{:: 
string;; 
newLangCode;; "
=;;# $
selectedOption;;% 3
.;;3 4
CultureCode;;4 ?
;;;? @
if== 
(== 
Lang== 
.== 
Culture==  
.==  !
Name==! %
!===& (
newLangCode==) 4
)==4 5
{>> 

Properties?? 
.?? 
Settings?? '
.??' (
Default??( /
.??/ 0
languageCode??0 <
=??= >
newLangCode??? J
;??J K

Properties@@ 
.@@ 
Settings@@ '
.@@' (
Default@@( /
.@@/ 0
Save@@0 4
(@@4 5
)@@5 6
;@@6 7
LangAA 
.AA 
CultureAA  
=AA! "
newAA# &
CultureInfoAA' 2
(AA2 3
newLangCodeAA3 >
)AA> ?
;AA? @
LocalizationHelperCC &
.CC& '
ApplyLanguageFontCC' 8
(CC8 9
)CC9 :
;CC: ;
_languageChangedEE $
=EE% &
trueEE' +
;EE+ ,
RefreshWindowFF !
(FF! "
)FF" #
;FF# $
}GG 
}HH 
}II 	
privateKK 
voidKK 
RefreshWindowKK "
(KK" #
)KK# $
{LL 	
varMM 
newSettingsWindowMM !
=MM" #
newMM$ '
SettingsMM( 0
(MM0 1
trueMM1 5
)MM5 6
;MM6 7
newSettingsWindowNN 
.NN 
OwnerNN #
=NN$ %
thisNN& *
.NN* +
OwnerNN+ 0
;NN0 1
NavigationHelperOO 
.OO 

NavigateToOO '
(OO' (
thisOO( ,
,OO, -
newSettingsWindowOO. ?
)OO? @
;OO@ A
}PP 	
privateRR 
voidRR 
ButtonBack_ClickRR %
(RR% &
objectRR& ,
senderRR- 3
,RR3 4
RoutedEventArgsRR5 D
eRRE F
)RRF G
{SS 	
ifTT 
(TT 
_languageChangedTT  
)TT  !
{UU 
NavigationHelperVV  
.VV  !

NavigateToVV! +
(VV+ ,
thisVV, 0
,VV0 1
newVV2 5
MainMenuVV6 >
(VV> ?
)VV? @
)VV@ A
;VVA B
}WW 
elseXX 
{YY 
NavigationHelperZZ  
.ZZ  !

NavigateToZZ! +
(ZZ+ ,
thisZZ, 0
,ZZ0 1
thisZZ2 6
.ZZ6 7
OwnerZZ7 <
??ZZ= ?
newZZ@ C
MainMenuZZD L
(ZZL M
)ZZM N
)ZZN O
;ZZO P
}[[ 
}\\ 	
}]] 
}^^ ÔR
<C:\MemoryGame\Client\Client\Views\Session\VerifyCode.xaml.cs
	namespace 	
Client
 
. 
Views 
. 
Session 
{ 
public 

partial 
class 

VerifyCode #
:$ %
Window& ,
{ 
private 
const 
int 

PIN_LENGTH $
=% &
$num' (
;( )
private 
readonly 
string 
_email  &
;& '
private 
readonly 
bool 
_isGuestRegister .
;. /
public 

VerifyCode 
( 
string  
email! &
,& '
bool( ,
isGuestRegister- <
== >
false? D
)D E
{ 	
InitializeComponent 
(  
)  !
;! "
_email 
= 
email 
?? 
throw #
new$ '!
ArgumentNullException( =
(= >
nameof> D
(D E
emailE J
)J K
)K L
;L M
_isGuestRegister 
= 
isGuestRegister .
;. /
LabelRegisterEmail 
. 
Content &
=' (
_email) /
;/ 0
}   	
private"" 
void"" (
NumericOnly_PreviewTextInput"" 1
(""1 2
object""2 8
sender""9 ?
,""? @$
TextCompositionEventArgs""A Y
e""Z [
)""[ \
{## 	
e$$ 
.$$ 
Handled$$ 
=$$ 
new$$ 
Regex$$ !
($$! "
$str$$" +
)$$+ ,
.$$, -
IsMatch$$- 4
($$4 5
e$$5 6
.$$6 7
Text$$7 ;
)$$; <
;$$< =
}%% 	
private'' 
void'' "
TextBox_PreviewKeyDown'' +
(''+ ,
object'', 2
sender''3 9
,''9 :
KeyEventArgs''; G
e''H I
)''I J
{(( 	
if)) 
()) 
e)) 
.)) 
Key)) 
==)) 
Key)) 
.)) 
Space)) "
)))" #
{** 
e++ 
.++ 
Handled++ 
=++ 
true++  
;++  !
},, 
}-- 	
private// 
async// 
void// "
ButtonVerifyCode_Click// 1
(//1 2
object//2 8
sender//9 ?
,//? @
RoutedEventArgs//A P
e//Q R
)//R S
{00 	
string11 
code11 
=11 &
TextBoxInputVericationCode11 4
.114 5
Text115 9
?119 :
.11: ;
Trim11; ?
(11? @
)11@ A
;11A B
LabelCodeError22 
.22 
Content22 "
=22# $
$str22% '
;22' (
ValidationCode44 
validationCode44 )
=44* +
ValidateVerifyCode44, >
(44> ?
code44? C
,44C D

PIN_LENGTH44E O
)44O P
;44P Q
if55 
(55 
validationCode55 
!=55 !
ValidationCode55" 0
.550 1
Success551 8
)558 9
{66 
LabelCodeError77 
.77 
Content77 &
=77' (
	GetString77) 2
(772 3
validationCode773 A
)77A B
;77B C
return88 
;88 
}99 
ButtonVerifyCode;; 
.;; 
	IsEnabled;; &
=;;' (
false;;) .
;;;. /
try== 
{>> 
ResponseDTO?? 
response?? $
;??$ %
string@@ 
messageSuccess@@ %
=@@& '
Lang@@( ,
.@@, -&
VerifyCode_Message_Success@@- G
;@@G H
ifBB 
(BB 
_isGuestRegisterBB $
)BB$ %
{CC 
responseDD 
=DD 
awaitDD $
UserServiceManagerDD% 7
.DD7 8
InstanceDD8 @
.DD@ A
ClientDDA G
.DDG H(
VerifyGuestRegistrationAsyncDDH d
(DDd e
UserSessionDDe p
.DDp q
UserIdDDq w
,DDw x
_emailDDy 
,	DD Ä
code
DDÅ Ö
)
DDÖ Ü
;
DDÜ á
messageSuccessEE "
=EE# $
LangEE% )
.EE) *+
VerifyCode_Message_GuestSuccessEE* I
;EEI J
}FF 
elseGG 
{HH 
responseII 
=II 
awaitII $
UserServiceManagerII% 7
.II7 8
InstanceII8 @
.II@ A
ClientIIA G
.IIG H#
VerifyRegistrationAsyncIIH _
(II_ `
_emailII` f
,IIf g
codeIIh l
)IIl m
;IIm n
}JJ 
ifLL 
(LL 
responseLL 
.LL 
SuccessLL $
)LL$ %
{MM 
newNN 
CustomMessageBoxNN (
(NN( )
LangOO 
.OO  
Global_Title_SuccessOO 1
,OO1 2
messageSuccessOO3 A
,OOA B
thisPP 
,PP 
MessageBoxTypePP ,
.PP, -
SuccessPP- 4
)PP4 5
.PP5 6

ShowDialogPP6 @
(PP@ A
)PPA B
;PPB C
ifRR 
(RR 
_isGuestRegisterRR (
)RR( )
{SS 
UserSessionTT #
.TT# $

EndSessionTT$ .
(TT. /
)TT/ 0
;TT0 1
NavigationHelperUU (
.UU( )

NavigateToUU) 3
(UU3 4
thisUU4 8
,UU8 9
newUU: =
LoginUU> C
(UUC D
)UUD E
)UUE F
;UUF G
}VV 
elseWW 
{XX 
NavigationHelperYY (
.YY( )

NavigateToYY) 3
(YY3 4
thisYY4 8
,YY8 9
newYY: =
SetupProfileYY> J
(YYJ K
_emailYYK Q
)YYQ R
)YYR S
;YYS T
}ZZ 
}[[ 
else\\ 
{]] 
string^^ 
errorMessage^^ '
=^^( )
	GetString^^* 3
(^^3 4
response^^4 <
.^^< =

MessageKey^^= G
)^^G H
;^^H I
var__ 
msgBox__ 
=__  
new__! $
CustomMessageBox__% 5
(__5 6
Lang`` 
.`` 
Global_Title_Error`` /
,``/ 0
errorMessage``1 =
,``= >
thisaa 
,aa 
MessageBoxTypeaa ,
.aa, -
Erroraa- 2
)aa2 3
;aa3 4
msgBoxbb 
.bb 

ShowDialogbb %
(bb% &
)bb& '
;bb' (
ButtonVerifyCodedd $
.dd$ %
	IsEnableddd% .
=dd/ 0
truedd1 5
;dd5 6
}ee 
}ff 
catchgg 
(gg 
	Exceptiongg 
exgg 
)gg  
{hh 
ExceptionManagerii  
.ii  !
Handleii! '
(ii' (
exii( *
,ii* +
thisii, 0
,ii0 1
(ii2 3
)ii3 4
=>ii5 7
ButtonVerifyCodeii8 H
.iiH I
	IsEnablediiI R
=iiS T
trueiiU Y
)iiY Z
;iiZ [
}jj 
}kk 	
privatemm 
asyncmm 
voidmm .
"ButtonResendVerificationCode_Clickmm =
(mm= >
objectmm> D
sendermmE K
,mmK L
RoutedEventArgsmmM \
emm] ^
)mm^ _
{nn 	
ButtonResendCodeoo 
.oo 
	IsEnabledoo &
=oo' (
falseoo) .
;oo. /
tryqq 
{rr 
ResponseDTOss 
responsess $
=ss% &
awaitss' ,
UserServiceManagerss- ?
.ss? @
Instancess@ H
.ssH I
ClientssI O
.ssO P'
ResendVerificationCodeAsyncssP k
(ssk l
_emailssl r
)ssr s
;sss t
ifuu 
(uu 
responseuu 
.uu 
Successuu $
)uu$ %
{vv 
newww 
CustomMessageBoxww (
(ww( )
Langxx 
.xx  
Global_Title_Successxx 1
,xx1 2
Langxx3 7
.xx7 8%
Verify_Message_CodeResentxx8 Q
,xxQ R
thisyy 
,yy 
MessageBoxTypeyy ,
.yy, -
Successyy- 4
)yy4 5
.yy5 6

ShowDialogyy6 @
(yy@ A
)yyA B
;yyB C
}zz 
else{{ 
{|| 
string}} 
errorMessage}} '
=}}( )
	GetString}}* 3
(}}3 4
response}}4 <
.}}< =

MessageKey}}= G
)}}G H
;}}H I
new~~ 
CustomMessageBox~~ (
(~~( )
Lang 
. 
Global_Title_Error /
,/ 0
errorMessage1 =
,= >
this
ÄÄ 
,
ÄÄ 
MessageBoxType
ÄÄ ,
.
ÄÄ, -
Error
ÄÄ- 2
)
ÄÄ2 3
.
ÄÄ3 4

ShowDialog
ÄÄ4 >
(
ÄÄ> ?
)
ÄÄ? @
;
ÄÄ@ A
}
ÅÅ 
}
ÇÇ 
catch
ÉÉ 
(
ÉÉ 
	Exception
ÉÉ 
ex
ÉÉ 
)
ÉÉ  
{
ÑÑ 
ExceptionManager
ÖÖ  
.
ÖÖ  !
Handle
ÖÖ! '
(
ÖÖ' (
ex
ÖÖ( *
,
ÖÖ* +
this
ÖÖ, 0
)
ÖÖ0 1
;
ÖÖ1 2
}
ÜÜ 
finally
áá 
{
àà 
ButtonResendCode
ââ  
.
ââ  !
	IsEnabled
ââ! *
=
ââ+ ,
true
ââ- 1
;
ââ1 2
}
ää 
}
ãã 	
private
çç 
void
çç &
ButtonBackToSignIn_Click
çç -
(
çç- .
object
çç. 4
sender
çç5 ;
,
çç; <
RoutedEventArgs
çç= L
e
ççM N
)
ççN O
{
éé 	
NavigationHelper
èè 
.
èè 

NavigateTo
èè '
(
èè' (
this
èè( ,
,
èè, -
this
èè. 2
.
èè2 3
Owner
èè3 8
??
èè9 ;
new
èè< ?
RegisterAccount
èè@ O
(
èèO P
_isGuestRegister
èèP `
)
èè` a
)
èèa b
;
èèb c
}
êê 	
}
ëë 
}íí ‹‹
IC:\MemoryGame\Client\Client\Views\Multiplayer\PlayGameMultiplayer.xaml.cs
	namespace 	
Client
 
. 
Views 
. 
Multiplayer "
{ 
public 

partial 
class 
PlayGameMultiplayer ,
:- .
Window/ 5
{ 
private 
const 
int 
MAX_CHAT_MESSAGES +
=, -
$num. 0
;0 1
private 
readonly 
GameManager $
_gameManager% 1
;1 2
private 
readonly 
List 
< 
LobbyPlayerInfo -
>- .
_players/ 7
;7 8
private 
string 
_currentTurnPlayer )
;) *
private 
Border 
[ 
] 
_playerBorders '
;' (
private   
Label   
[   
]   
_playerNames   $
;  $ %
private!! 
Label!! 
[!! 
]!! 
_playerScores!! %
;!!% &
private"" 
Label"" 
["" 
]"" 
_playerTimes"" $
;""$ %
public&&  
ObservableCollection&& #
<&&# $
Card&&$ (
>&&( )
Cards&&* /
{&&0 1
get&&2 5
;&&5 6
set&&7 :
;&&: ;
}&&< =
public(( 
PlayGameMultiplayer(( "
(((" #
List((# '
<((' (
CardInfo((( 0
>((0 1
serverCards((2 =
,((= >
List((? C
<((C D
LobbyPlayerInfo((D S
>((S T
players((U \
)((\ ]
{)) 	
InitializeComponent** 
(**  
)**  !
;**! "
_players,, 
=,, 
players,, 
??,, !
new,," %
List,,& *
<,,* +
LobbyPlayerInfo,,+ :
>,,: ;
(,,; <
),,< =
;,,= >
Cards-- 
=-- 
new--  
ObservableCollection-- ,
<--, -
Card--- 1
>--1 2
(--2 3
)--3 4
;--4 5
InitializeUIArrays// 
(// 
)//  
;//  !
	GameBoard00 
.00 
ItemsSource00 !
=00" #
Cards00$ )
;00) *
this11 
.11 
DataContext11 
=11 
this11 #
;11# $
SetupPlayerUI33 
(33 
)33 
;33 
_gameManager55 
=55 
new55 
GameManager55 *
(55* +
Cards55+ 0
)550 1
;551 2
ConfigureEvents66 
(66 
)66 
;66 
if88 
(88 
_players88 
.88 
Count88 
>88  
$num88! "
)88" #
{99 
_currentTurnPlayer:: "
=::# $
_players::% -
[::- .
$num::. /
]::/ 0
.::0 1
Name::1 5
;::5 6!
HighlightActivePlayer;; %
(;;% &
_currentTurnPlayer;;& 8
);;8 9
;;;9 :
int<< 
activeIndex<< 
=<<  !
_players<<" *
.<<* +
	FindIndex<<+ 4
(<<4 5
p<<5 6
=><<7 9
p<<: ;
.<<; <
Name<<< @
==<<A C
_currentTurnPlayer<<D V
)<<V W
;<<W X
if== 
(== 
activeIndex== 
>===  "
$num==# $
&&==% '
activeIndex==( 3
<==4 5
_playerTimes==6 B
.==B C
Length==C I
)==I J
{>> 
_playerTimes??  
[??  !
activeIndex??! ,
]??, -
.??- .
Content??. 5
=??6 7
$"??8 :
$str??: @
{??@ A
GameConstants??A N
.??N O"
DefaultTurnTimeSeconds??O e
}??e f
"??f g
;??g h
}@@ 
}AA 
StartGameSafeCC 
(CC 
serverCardsCC %
)CC% &
;CC& '
}DD 	
privateFF 
voidFF 
InitializeUIArraysFF '
(FF' (
)FF( )
{GG 	
_playerBordersHH 
=HH 
newHH  
BorderHH! '
[HH' (
]HH( )
{HH* +
BorderP1HH, 4
,HH4 5
BorderP2HH6 >
,HH> ?
BorderP3HH@ H
,HHH I
BorderP4HHJ R
}HHS T
;HHT U
_playerNamesII 
=II 
newII 
LabelII $
[II$ %
]II% &
{II' (
LabelP1NameII) 4
,II4 5
LabelP2NameII6 A
,IIA B
LabelP3NameIIC N
,IIN O
LabelP4NameIIP [
}II\ ]
;II] ^
_playerScoresJJ 
=JJ 
newJJ 
LabelJJ  %
[JJ% &
]JJ& '
{JJ( )
LabelP1ScoreJJ* 6
,JJ6 7
LabelP2ScoreJJ8 D
,JJD E
LabelP3ScoreJJF R
,JJR S
LabelP4ScoreJJT `
}JJa b
;JJb c
_playerTimesKK 
=KK 
newKK 
LabelKK $
[KK$ %
]KK% &
{KK' (
LabelP1TimeKK) 4
,KK4 5
LabelP2TimeKK6 A
,KKA B
LabelP3TimeKKC N
,KKN O
LabelP4TimeKKP [
}KK\ ]
;KK] ^
}LL 	
privateNN 
voidNN 
SetupPlayerUINN "
(NN" #
)NN# $
{OO 	
forPP 
(PP 
intPP 
iPP 
=PP 
$numPP 
;PP 
iPP 
<PP 
_playerBordersPP  .
.PP. /
LengthPP/ 5
;PP5 6
iPP7 8
++PP8 :
)PP: ;
{QQ 
ifRR 
(RR 
iRR 
<RR 
_playersRR  
.RR  !
CountRR! &
)RR& '
{SS 
_playerBordersTT "
[TT" #
iTT# $
]TT$ %
.TT% &

VisibilityTT& 0
=TT1 2

VisibilityTT3 =
.TT= >
VisibleTT> E
;TTE F
_playerNamesUU  
[UU  !
iUU! "
]UU" #
.UU# $
ContentUU$ +
=UU, -
_playersUU. 6
[UU6 7
iUU7 8
]UU8 9
.UU9 :
NameUU: >
;UU> ?
_playerScoresVV !
[VV! "
iVV" #
]VV# $
.VV$ %
ContentVV% ,
=VV- .
$strVV/ 9
;VV9 :
_playerTimesWW  
[WW  !
iWW! "
]WW" #
.WW# $
ContentWW$ +
=WW, -
$strWW. 8
;WW8 9
_playerBordersYY "
[YY" #
iYY# $
]YY$ %
.YY% &
TagYY& )
=YY* +
_playersYY, 4
[YY4 5
iYY5 6
]YY6 7
.YY7 8
NameYY8 <
;YY< =
if[[ 
([[ 
_players[[  
[[[  !
i[[! "
][[" #
.[[# $
Name[[$ (
==[[) +
UserSession[[, 7
.[[7 8
Username[[8 @
)[[@ A
{\\ 
_playerNames]] $
[]]$ %
i]]% &
]]]& '
.]]' (

Foreground]]( 2
=]]3 4
Brushes]]5 <
.]]< =
Gold]]= A
;]]A B
_playerNames^^ $
[^^$ %
i^^% &
]^^& '
.^^' (

FontWeight^^( 2
=^^3 4
FontWeights^^5 @
.^^@ A
Bold^^A E
;^^E F
}__ 
}`` 
elseaa 
{bb 
_playerBorderscc "
[cc" #
icc# $
]cc$ %
.cc% &

Visibilitycc& 0
=cc1 2

Visibilitycc3 =
.cc= >
	Collapsedcc> G
;ccG H
}dd 
}ee 
}ff 	
privatehh 
voidhh 
StartGameSafehh "
(hh" #
Listhh# '
<hh' (
CardInfohh( 0
>hh0 1
serverCardshh2 =
)hh= >
{ii 	
tryjj 
{kk 
varll 
configll 
=ll 
newll  
GameConfigurationll! 2
{mm 
NumberOfCardsnn !
=nn" #
serverCardsnn$ /
.nn/ 0
Countnn0 5
,nn5 6
TimeLimitSecondsoo $
=oo% &
GameConstantsoo' 4
.oo4 5"
DefaultTurnTimeSecondsoo5 K
}pp 
;pp 
_gameManagerqq 
.qq  
StartMultiplayerGameqq 1
(qq1 2
configqq2 8
,qq8 9
serverCardsqq: E
)qqE F
;qqF G
}rr 
catchss 
(ss 
	Exceptionss 
exss 
)ss  
{tt 
ExceptionManageruu  
.uu  !
Handleuu! '
(uu' (
exuu( *
,uu* +
thisuu, 0
,uu0 1
(uu2 3
)uu3 4
=>uu5 7
thisuu8 <
.uu< =
Closeuu= B
(uuB C
)uuC D
)uuD E
;uuE F
}vv 
}ww 	
private{{ 
void{{ 
ConfigureEvents{{ $
({{$ %
){{% &
{|| 	
_gameManager}} 
.}} 
TimerUpdated}} %
+=}}& (
OnLocalTimerTick}}) 9
;}}9 :
var 
service 
= 
GameServiceManager ,
., -
Instance- 5
;5 6
service
ÄÄ 
.
ÄÄ 
TurnUpdated
ÄÄ 
+=
ÄÄ  "!
OnServerTurnUpdated
ÄÄ# 6
;
ÄÄ6 7
service
ÅÅ 
.
ÅÅ 
	CardShown
ÅÅ 
+=
ÅÅ  
OnServerCardShown
ÅÅ! 2
;
ÅÅ2 3
service
ÇÇ 
.
ÇÇ 
CardsHidden
ÇÇ 
+=
ÇÇ  "!
OnServerCardsHidden
ÇÇ# 6
;
ÇÇ6 7
service
ÉÉ 
.
ÉÉ 
CardsMatched
ÉÉ  
+=
ÉÉ! #"
OnServerCardsMatched
ÉÉ$ 8
;
ÉÉ8 9
service
ÑÑ 
.
ÑÑ 
ScoreUpdated
ÑÑ  
+=
ÑÑ! #"
OnServerScoreUpdated
ÑÑ$ 8
;
ÑÑ8 9
service
ÖÖ 
.
ÖÖ 
GameFinished
ÖÖ  
+=
ÖÖ! #"
OnServerGameFinished
ÖÖ$ 8
;
ÖÖ8 9
service
ÜÜ 
.
ÜÜ !
ChatMessageReceived
ÜÜ '
+=
ÜÜ( *#
OnChatMessageReceived
ÜÜ+ @
;
ÜÜ@ A
service
áá 
.
áá 

PlayerLeft
áá 
+=
áá !
OnPlayerLeft
áá" .
;
áá. /
}
àà 	
private
ää 
void
ää 
UnsubscribeEvents
ää &
(
ää& '
)
ää' (
{
ãã 	
_gameManager
åå 
.
åå 
TimerUpdated
åå %
-=
åå& (
OnLocalTimerTick
åå) 9
;
åå9 :
var
éé 
service
éé 
=
éé  
GameServiceManager
éé ,
.
éé, -
Instance
éé- 5
;
éé5 6
service
èè 
.
èè 
TurnUpdated
èè 
-=
èè  "!
OnServerTurnUpdated
èè# 6
;
èè6 7
service
êê 
.
êê 
	CardShown
êê 
-=
êê  
OnServerCardShown
êê! 2
;
êê2 3
service
ëë 
.
ëë 
CardsHidden
ëë 
-=
ëë  "!
OnServerCardsHidden
ëë# 6
;
ëë6 7
service
íí 
.
íí 
CardsMatched
íí  
-=
íí! #"
OnServerCardsMatched
íí$ 8
;
íí8 9
service
ìì 
.
ìì 
ScoreUpdated
ìì  
-=
ìì! #"
OnServerScoreUpdated
ìì$ 8
;
ìì8 9
service
îî 
.
îî 
GameFinished
îî  
-=
îî! #"
OnServerGameFinished
îî$ 8
;
îî8 9
service
ïï 
.
ïï !
ChatMessageReceived
ïï '
-=
ïï( *#
OnChatMessageReceived
ïï+ @
;
ïï@ A
service
ññ 
.
ññ 

PlayerLeft
ññ 
-=
ññ !
OnPlayerLeft
ññ" .
;
ññ. /
}
óó 	
private
ùù 
void
ùù !
OnServerTurnUpdated
ùù (
(
ùù( )
string
ùù) /
nextPlayerName
ùù0 >
,
ùù> ?
int
ùù@ C
seconds
ùùD K
)
ùùK L
{
ûû 	

Dispatcher
üü 
.
üü 
Invoke
üü 
(
üü 
(
üü 
)
üü  
=>
üü! #
{
†† 
if
°° 
(
°° 
!
°° 
this
°° 
.
°° 
IsLoaded
°° "
)
°°" #
{
¢¢ 
return
££ 
;
££ 
}
§§  
_currentTurnPlayer
¶¶ "
=
¶¶# $
nextPlayerName
¶¶% 3
;
¶¶3 4
foreach
®® 
(
®® 
var
®® 
label
®® "
in
®®# %
_playerTimes
®®& 2
)
®®2 3
{
©© 
if
™™ 
(
™™ 
label
™™ 
!=
™™  
null
™™! %
&&
™™& (
label
™™) .
.
™™. /

Visibility
™™/ 9
==
™™: <

Visibility
™™= G
.
™™G H
Visible
™™H O
)
™™O P
{
´´ 
label
¨¨ 
.
¨¨ 
Content
¨¨ %
=
¨¨& '
$str
¨¨( 2
;
¨¨2 3
}
≠≠ 
}
ÆÆ #
HighlightActivePlayer
∞∞ %
(
∞∞% &
nextPlayerName
∞∞& 4
)
∞∞4 5
;
∞∞5 6
_gameManager
≤≤ 
.
≤≤  
UpdateTurnDuration
≤≤ /
(
≤≤/ 0
seconds
≤≤0 7
)
≤≤7 8
;
≤≤8 9
_gameManager
≥≥ 
.
≥≥ 
ResetTurnTimer
≥≥ +
(
≥≥+ ,
)
≥≥, -
;
≥≥- .
}
¥¥ 
)
¥¥ 
;
¥¥ 
}
µµ 	
private
∑∑ 
void
∑∑ 
OnServerCardShown
∑∑ &
(
∑∑& '
int
∑∑' *
	cardIndex
∑∑+ 4
,
∑∑4 5
string
∑∑6 <
imageId
∑∑= D
)
∑∑D E
{
∏∏ 	

Dispatcher
ππ 
.
ππ 
Invoke
ππ 
(
ππ 
(
ππ 
)
ππ  
=>
ππ! #
{
∫∫ 
if
ªª 
(
ªª 
!
ªª 
this
ªª 
.
ªª 
IsLoaded
ªª "
)
ªª" #
{
ºº 
return
ΩΩ 
;
ΩΩ 
}
ææ 
var
¿¿ 
card
¿¿ 
=
¿¿ 
Cards
¿¿  
.
¿¿  !
FirstOrDefault
¿¿! /
(
¿¿/ 0
c
¿¿0 1
=>
¿¿2 4
c
¿¿5 6
.
¿¿6 7
Id
¿¿7 9
==
¿¿: <
	cardIndex
¿¿= F
)
¿¿F G
;
¿¿G H
if
¡¡ 
(
¡¡ 
card
¡¡ 
!=
¡¡ 
null
¡¡  
)
¡¡  !
{
¬¬ 
card
√√ 
.
√√ 
	IsFlipped
√√ "
=
√√# $
true
√√% )
;
√√) *
}
ƒƒ 
}
≈≈ 
)
≈≈ 
;
≈≈ 
}
∆∆ 	
private
»» 
void
»» !
OnServerCardsHidden
»» (
(
»»( )
int
»») ,
idx1
»»- 1
,
»»1 2
int
»»3 6
idx2
»»7 ;
)
»»; <
{
…… 	

Dispatcher
   
.
   
Invoke
   
(
   
(
   
)
    
=>
  ! #
{
ÀÀ 
if
ÃÃ 
(
ÃÃ 
!
ÃÃ 
this
ÃÃ 
.
ÃÃ 
IsLoaded
ÃÃ "
)
ÃÃ" #
{
ÕÕ 
return
ŒŒ 
;
ŒŒ 
}
œœ 
var
—— 
c1
—— 
=
—— 
Cards
—— 
.
—— 
FirstOrDefault
—— -
(
——- .
c
——. /
=>
——0 2
c
——3 4
.
——4 5
Id
——5 7
==
——8 :
idx1
——; ?
)
——? @
;
——@ A
var
““ 
c2
““ 
=
““ 
Cards
““ 
.
““ 
FirstOrDefault
““ -
(
““- .
c
““. /
=>
““0 2
c
““3 4
.
““4 5
Id
““5 7
==
““8 :
idx2
““; ?
)
““? @
;
““@ A
if
‘‘ 
(
‘‘ 
c1
‘‘ 
!=
‘‘ 
null
‘‘ 
)
‘‘ 
{
’’ 
c1
÷÷ 
.
÷÷ 
	IsFlipped
÷÷  
=
÷÷! "
false
÷÷# (
;
÷÷( )
}
◊◊ 
if
ÿÿ 
(
ÿÿ 
c2
ÿÿ 
!=
ÿÿ 
null
ÿÿ 
)
ÿÿ 
{
ŸŸ 
c2
⁄⁄ 
.
⁄⁄ 
	IsFlipped
⁄⁄  
=
⁄⁄! "
false
⁄⁄# (
;
⁄⁄( )
}
€€ 
}
‹‹ 
)
‹‹ 
;
‹‹ 
}
›› 	
private
ﬂﬂ 
void
ﬂﬂ "
OnServerCardsMatched
ﬂﬂ )
(
ﬂﬂ) *
int
ﬂﬂ* -
idx1
ﬂﬂ. 2
,
ﬂﬂ2 3
int
ﬂﬂ4 7
idx2
ﬂﬂ8 <
)
ﬂﬂ< =
{
‡‡ 	

Dispatcher
·· 
.
·· 
Invoke
·· 
(
·· 
(
·· 
)
··  
=>
··! #
{
‚‚ 
if
„„ 
(
„„ 
!
„„ 
this
„„ 
.
„„ 
IsLoaded
„„ "
)
„„" #
{
‰‰ 
return
ÂÂ 
;
ÂÂ 
}
ÊÊ 
var
ËË 
c1
ËË 
=
ËË 
Cards
ËË 
.
ËË 
FirstOrDefault
ËË -
(
ËË- .
c
ËË. /
=>
ËË0 2
c
ËË3 4
.
ËË4 5
Id
ËË5 7
==
ËË8 :
idx1
ËË; ?
)
ËË? @
;
ËË@ A
var
ÈÈ 
c2
ÈÈ 
=
ÈÈ 
Cards
ÈÈ 
.
ÈÈ 
FirstOrDefault
ÈÈ -
(
ÈÈ- .
c
ÈÈ. /
=>
ÈÈ0 2
c
ÈÈ3 4
.
ÈÈ4 5
Id
ÈÈ5 7
==
ÈÈ8 :
idx2
ÈÈ; ?
)
ÈÈ? @
;
ÈÈ@ A
if
ÎÎ 
(
ÎÎ 
c1
ÎÎ 
!=
ÎÎ 
null
ÎÎ 
)
ÎÎ 
{
ÏÏ 
c1
ÌÌ 
.
ÌÌ 
	IsMatched
ÌÌ  
=
ÌÌ! "
true
ÌÌ# '
;
ÌÌ' (
c1
ÓÓ 
.
ÓÓ 
	IsFlipped
ÓÓ  
=
ÓÓ! "
true
ÓÓ# '
;
ÓÓ' (
}
ÔÔ 
if
 
(
 
c2
 
!=
 
null
 
)
 
{
ÒÒ 
c2
ÚÚ 
.
ÚÚ 
	IsMatched
ÚÚ  
=
ÚÚ! "
true
ÚÚ# '
;
ÚÚ' (
c2
ÛÛ 
.
ÛÛ 
	IsFlipped
ÛÛ  
=
ÛÛ! "
true
ÛÛ# '
;
ÛÛ' (
}
ÙÙ 
}
ıı 
)
ıı 
;
ıı 
}
ˆˆ 	
private
¯¯ 
void
¯¯ "
OnServerScoreUpdated
¯¯ )
(
¯¯) *
string
¯¯* 0

playerName
¯¯1 ;
,
¯¯; <
int
¯¯= @
score
¯¯A F
)
¯¯F G
{
˘˘ 	

Dispatcher
˙˙ 
.
˙˙ 
Invoke
˙˙ 
(
˙˙ 
(
˙˙ 
)
˙˙  
=>
˙˙! #
{
˚˚ 
if
¸¸ 
(
¸¸ 
!
¸¸ 
this
¸¸ 
.
¸¸ 
IsLoaded
¸¸ "
)
¸¸" #
{
˝˝ 
return
˛˛ 
;
˛˛ 
}
ˇˇ 
int
ÅÅ 
index
ÅÅ 
=
ÅÅ 
_players
ÅÅ $
.
ÅÅ$ %
	FindIndex
ÅÅ% .
(
ÅÅ. /
p
ÅÅ/ 0
=>
ÅÅ1 3
p
ÅÅ4 5
.
ÅÅ5 6
Name
ÅÅ6 :
==
ÅÅ; =

playerName
ÅÅ> H
)
ÅÅH I
;
ÅÅI J
if
ÇÇ 
(
ÇÇ 
index
ÇÇ 
>=
ÇÇ 
$num
ÇÇ 
&&
ÇÇ !
index
ÇÇ" '
<
ÇÇ( )
_playerScores
ÇÇ* 7
.
ÇÇ7 8
Length
ÇÇ8 >
)
ÇÇ> ?
{
ÉÉ 
_playerScores
ÑÑ !
[
ÑÑ! "
index
ÑÑ" '
]
ÑÑ' (
.
ÑÑ( )
Content
ÑÑ) 0
=
ÑÑ1 2
$"
ÑÑ3 5
$str
ÑÑ5 <
{
ÑÑ< =
score
ÑÑ= B
}
ÑÑB C
"
ÑÑC D
;
ÑÑD E
}
ÖÖ 
}
ÜÜ 
)
ÜÜ 
;
ÜÜ 
}
áá 	
private
ââ 
void
ââ "
OnServerGameFinished
ââ )
(
ââ) *
string
ââ* 0

winnerName
ââ1 ;
)
ââ; <
{
ää 	

Dispatcher
ãã 
.
ãã 
Invoke
ãã 
(
ãã 
(
ãã 
)
ãã  
=>
ãã! #
{
åå 
if
çç 
(
çç 
!
çç 
this
çç 
.
çç 
IsLoaded
çç "
)
çç" #
{
éé 
return
èè 
;
èè 
}
êê 
string
íí 
myScoreText
íí "
=
íí# $
GetMyCurrentScore
íí% 6
(
íí6 7
)
íí7 8
;
íí8 9
ShowMatchSummary
ìì  
(
ìì  !
$"
ìì! #
{
ìì# $

winnerName
ìì$ .
}
ìì. /
$str
ìì/ 5
"
ìì5 6
,
ìì6 7
$"
ìì8 :
{
ìì: ;
Lang
ìì; ?
.
ìì? @ 
Global_Label_Score
ìì@ R
}
ììR S
$str
ììS U
{
ììU V
myScoreText
ììV a
}
ììa b
"
ììb c
)
ììc d
;
ììd e
}
îî 
)
îî 
;
îî 
}
ïï 	
private
õõ 
void
õõ 
OnLocalTimerTick
õõ %
(
õõ% &
string
õõ& ,

timeString
õõ- 7
)
õõ7 8
{
úú 	

Dispatcher
ùù 
.
ùù 
Invoke
ùù 
(
ùù 
(
ùù 
)
ùù  
=>
ùù! #
{
ûû 
if
üü 
(
üü 
!
üü 
this
üü 
.
üü 
IsLoaded
üü "
)
üü" #
{
†† 
return
°° 
;
°° 
}
¢¢ 
int
§§ 
activeIndex
§§ 
=
§§  !
_players
§§" *
.
§§* +
	FindIndex
§§+ 4
(
§§4 5
p
§§5 6
=>
§§7 9
p
§§: ;
.
§§; <
Name
§§< @
==
§§A C 
_currentTurnPlayer
§§D V
)
§§V W
;
§§W X
if
•• 
(
•• 
activeIndex
•• 
>=
••  "
$num
••# $
&&
••% '
activeIndex
••( 3
<
••4 5
_playerTimes
••6 B
.
••B C
Length
••C I
)
••I J
{
¶¶ 
_playerTimes
ßß  
[
ßß  !
activeIndex
ßß! ,
]
ßß, -
.
ßß- .
Content
ßß. 5
=
ßß6 7
$"
ßß8 :
$str
ßß: @
{
ßß@ A

timeString
ßßA K
}
ßßK L
"
ßßL M
;
ßßM N
}
®® 
}
©© 
)
©© 
;
©© 
}
™™ 	
private
¨¨ 
async
¨¨ 
void
¨¨ 

Card_Click
¨¨ %
(
¨¨% &
object
¨¨& ,
sender
¨¨- 3
,
¨¨3 4
RoutedEventArgs
¨¨5 D
e
¨¨E F
)
¨¨F G
{
≠≠ 	
if
ÆÆ 
(
ÆÆ  
_currentTurnPlayer
ÆÆ "
!=
ÆÆ# %
UserSession
ÆÆ& 1
.
ÆÆ1 2
Username
ÆÆ2 :
)
ÆÆ: ;
{
ØØ 
return
∞∞ 
;
∞∞ 
}
±± 
if
≥≥ 
(
≥≥ 
sender
≥≥ 
is
≥≥ 
Button
≥≥  
button
≥≥! '
&&
≥≥( *
button
≥≥+ 1
.
≥≥1 2
DataContext
≥≥2 =
is
≥≥> @
Card
≥≥A E
clickedCard
≥≥F Q
)
≥≥Q R
{
¥¥ 
if
µµ 
(
µµ 
clickedCard
µµ 
.
µµ  
	IsFlipped
µµ  )
||
µµ* ,
clickedCard
µµ- 8
.
µµ8 9
	IsMatched
µµ9 B
)
µµB C
{
∂∂ 
return
∑∑ 
;
∑∑ 
}
∏∏ 
try
∫∫ 
{
ªª 
await
ºº  
GameServiceManager
ºº ,
.
ºº, -
Instance
ºº- 5
.
ºº5 6
Client
ºº6 <
.
ºº< =
FlipCardAsync
ºº= J
(
ººJ K
clickedCard
ººK V
.
ººV W
Id
ººW Y
)
ººY Z
;
ººZ [
}
ΩΩ 
catch
ææ 
(
ææ 
	Exception
ææ  
ex
ææ! #
)
ææ# $
{
øø 
Debug
¿¿ 
.
¿¿ 
	WriteLine
¿¿ #
(
¿¿# $
$"
¿¿$ &
$str
¿¿& :
{
¿¿: ;
ex
¿¿; =
.
¿¿= >
Message
¿¿> E
}
¿¿E F
"
¿¿F G
)
¿¿G H
;
¿¿H I
ExceptionManager
¡¡ $
.
¡¡$ %
Handle
¡¡% +
(
¡¡+ ,
ex
¡¡, .
,
¡¡. /
this
¡¡0 4
)
¡¡4 5
;
¡¡5 6
}
¬¬ 
}
√√ 
}
ƒƒ 	
private
∆∆ 
void
∆∆ #
HighlightActivePlayer
∆∆ *
(
∆∆* +
string
∆∆+ 1

playerName
∆∆2 <
)
∆∆< =
{
«« 	
for
»» 
(
»» 
int
»» 
i
»» 
=
»» 
$num
»» 
;
»» 
i
»» 
<
»» 
_playerNames
»»  ,
.
»», -
Length
»»- 3
;
»»3 4
i
»»5 6
++
»»6 8
)
»»8 9
{
…… 
if
   
(
   
i
   
<
   
_players
    
.
    !
Count
  ! &
)
  & '
{
ÀÀ 
if
ÃÃ 
(
ÃÃ 
_players
ÃÃ  
[
ÃÃ  !
i
ÃÃ! "
]
ÃÃ" #
.
ÃÃ# $
Name
ÃÃ$ (
==
ÃÃ) +

playerName
ÃÃ, 6
)
ÃÃ6 7
{
ÕÕ 
_playerNames
ŒŒ $
[
ŒŒ$ %
i
ŒŒ% &
]
ŒŒ& '
.
ŒŒ' (

Foreground
ŒŒ( 2
=
ŒŒ3 4
Brushes
ŒŒ5 <
.
ŒŒ< =
Gold
ŒŒ= A
;
ŒŒA B
_playerNames
œœ $
[
œœ$ %
i
œœ% &
]
œœ& '
.
œœ' (

FontWeight
œœ( 2
=
œœ3 4
FontWeights
œœ5 @
.
œœ@ A
Bold
œœA E
;
œœE F
_playerBorders
—— &
[
——& '
i
——' (
]
——( )
.
——) *
BorderBrush
——* 5
=
——6 7
Brushes
——8 ?
.
——? @
Gold
——@ D
;
——D E
_playerBorders
““ &
[
““& '
i
““' (
]
““( )
.
““) *
BorderThickness
““* 9
=
““: ;
new
““< ?
	Thickness
““@ I
(
““I J
$num
““J K
)
““K L
;
““L M
}
”” 
else
‘‘ 
{
’’ 
_playerNames
÷÷ $
[
÷÷$ %
i
÷÷% &
]
÷÷& '
.
÷÷' (

Foreground
÷÷( 2
=
÷÷3 4
Brushes
÷÷5 <
.
÷÷< =
White
÷÷= B
;
÷÷B C
_playerNames
◊◊ $
[
◊◊$ %
i
◊◊% &
]
◊◊& '
.
◊◊' (

FontWeight
◊◊( 2
=
◊◊3 4
FontWeights
◊◊5 @
.
◊◊@ A
Normal
◊◊A G
;
◊◊G H
_playerBorders
ŸŸ &
[
ŸŸ& '
i
ŸŸ' (
]
ŸŸ( )
.
ŸŸ) *
BorderBrush
ŸŸ* 5
=
ŸŸ6 7
Brushes
ŸŸ8 ?
.
ŸŸ? @
Transparent
ŸŸ@ K
;
ŸŸK L
_playerBorders
⁄⁄ &
[
⁄⁄& '
i
⁄⁄' (
]
⁄⁄( )
.
⁄⁄) *
BorderThickness
⁄⁄* 9
=
⁄⁄: ;
new
⁄⁄< ?
	Thickness
⁄⁄@ I
(
⁄⁄I J
$num
⁄⁄J K
)
⁄⁄K L
;
⁄⁄L M
}
€€ 
}
‹‹ 
}
›› 
}
ﬁﬁ 	
private
‡‡ 
string
‡‡ 
GetMyCurrentScore
‡‡ (
(
‡‡( )
)
‡‡) *
{
·· 	
int
‚‚ 
myIndex
‚‚ 
=
‚‚ 
_players
‚‚ "
.
‚‚" #
	FindIndex
‚‚# ,
(
‚‚, -
p
‚‚- .
=>
‚‚/ 1
p
‚‚2 3
.
‚‚3 4
Name
‚‚4 8
==
‚‚9 ;
UserSession
‚‚< G
.
‚‚G H
Username
‚‚H P
)
‚‚P Q
;
‚‚Q R
if
„„ 
(
„„ 
myIndex
„„ 
>=
„„ 
$num
„„ 
&&
„„ 
myIndex
„„  '
<
„„( )
_playerScores
„„* 7
.
„„7 8
Length
„„8 >
)
„„> ?
{
‰‰ 
return
ÂÂ 
_playerScores
ÂÂ $
[
ÂÂ$ %
myIndex
ÂÂ% ,
]
ÂÂ, -
.
ÂÂ- .
Content
ÂÂ. 5
.
ÂÂ5 6
ToString
ÂÂ6 >
(
ÂÂ> ?
)
ÂÂ? @
;
ÂÂ@ A
}
ÊÊ 
return
ÁÁ 
$str
ÁÁ 
;
ÁÁ 
}
ËË 	
private
ÓÓ 
void
ÓÓ #
OnChatMessageReceived
ÓÓ *
(
ÓÓ* +
string
ÓÓ+ 1
sender
ÓÓ2 8
,
ÓÓ8 9
string
ÓÓ: @
message
ÓÓA H
,
ÓÓH I
bool
ÓÓJ N
isNotification
ÓÓO ]
)
ÓÓ] ^
{
ÔÔ 	

Dispatcher
 
.
 
Invoke
 
(
 
(
 
)
  
=>
! #
{
ÒÒ 
if
ÚÚ 
(
ÚÚ 
!
ÚÚ 
this
ÚÚ 
.
ÚÚ 
IsLoaded
ÚÚ "
)
ÚÚ" #
{
ÛÛ 
return
ÙÙ 
;
ÙÙ 
}
ıı 
string
˜˜ 
formattedMsg
˜˜ #
=
˜˜$ %
isNotification
˜˜& 4
?
˜˜5 6
$"
˜˜7 9
$str
˜˜9 =
{
˜˜= >
message
˜˜> E
}
˜˜E F
$str
˜˜F J
"
˜˜J K
:
˜˜L M
$"
˜˜N P
{
˜˜P Q
sender
˜˜Q W
}
˜˜W X
$str
˜˜X Z
{
˜˜Z [
message
˜˜[ b
}
˜˜b c
"
˜˜c d
;
˜˜d e
ChatListBox
¯¯ 
.
¯¯ 
Items
¯¯ !
.
¯¯! "
Add
¯¯" %
(
¯¯% &
formattedMsg
¯¯& 2
)
¯¯2 3
;
¯¯3 4
if
˙˙ 
(
˙˙ 
ChatListBox
˙˙ 
.
˙˙  
Items
˙˙  %
.
˙˙% &
Count
˙˙& +
>
˙˙, -
MAX_CHAT_MESSAGES
˙˙. ?
)
˙˙? @
{
˚˚ 
ChatListBox
¸¸ 
.
¸¸  
Items
¸¸  %
.
¸¸% &
RemoveAt
¸¸& .
(
¸¸. /
$num
¸¸/ 0
)
¸¸0 1
;
¸¸1 2
}
˝˝ 
if
ˇˇ 
(
ˇˇ 
ChatListBox
ˇˇ 
.
ˇˇ  
Items
ˇˇ  %
.
ˇˇ% &
Count
ˇˇ& +
>
ˇˇ, -
$num
ˇˇ. /
)
ˇˇ/ 0
{
ÄÄ 
ChatListBox
ÅÅ 
.
ÅÅ  
ScrollIntoView
ÅÅ  .
(
ÅÅ. /
ChatListBox
ÅÅ/ :
.
ÅÅ: ;
Items
ÅÅ; @
[
ÅÅ@ A
ChatListBox
ÅÅA L
.
ÅÅL M
Items
ÅÅM R
.
ÅÅR S
Count
ÅÅS X
-
ÅÅY Z
$num
ÅÅ[ \
]
ÅÅ\ ]
)
ÅÅ] ^
;
ÅÅ^ _
}
ÇÇ 
}
ÉÉ 
)
ÉÉ 
;
ÉÉ 
}
ÑÑ 	
private
ÜÜ 
void
ÜÜ 
OnPlayerLeft
ÜÜ !
(
ÜÜ! "
string
ÜÜ" (

playerName
ÜÜ) 3
)
ÜÜ3 4
{
áá 	

Dispatcher
àà 
.
àà 
Invoke
àà 
(
àà 
(
àà 
)
àà  
=>
àà! #
{
ââ 
if
ää 
(
ää 
!
ää 
this
ää 
.
ää 
IsLoaded
ää "
)
ää" #
{
ãã 
return
åå 
;
åå 
}
çç 
ChatListBox
èè 
.
èè 
Items
èè !
.
èè! "
Add
èè" %
(
èè% &
$"
èè& (
$str
èè( ,
{
èè, -

playerName
èè- 7
}
èè7 8
$str
èè8 J
"
èèJ K
)
èèK L
;
èèL M
int
êê 
index
êê 
=
êê 
_players
êê $
.
êê$ %
	FindIndex
êê% .
(
êê. /
p
êê/ 0
=>
êê1 3
p
êê4 5
.
êê5 6
Name
êê6 :
==
êê; =

playerName
êê> H
)
êêH I
;
êêI J
if
íí 
(
íí 
index
íí 
>=
íí 
$num
íí 
&&
íí !
index
íí" '
<
íí( )
_playerBorders
íí* 8
.
íí8 9
Length
íí9 ?
)
íí? @
{
ìì 
_playerBorders
îî "
[
îî" #
index
îî# (
]
îî( )
.
îî) *
Opacity
îî* 1
=
îî2 3
$num
îî4 7
;
îî7 8
_playerNames
ïï  
[
ïï  !
index
ïï! &
]
ïï& '
.
ïï' (
Content
ïï( /
+=
ïï0 2
$str
ïï3 <
;
ïï< =
_playerBorders
ññ "
[
ññ" #
index
ññ# (
]
ññ( )
.
ññ) *
ContextMenu
ññ* 5
=
ññ6 7
null
ññ8 <
;
ññ< =
}
óó 
}
òò 
)
òò 
;
òò 
}
ôô 	
private
õõ 
async
õõ 
void
õõ )
ButtonSendMessageChat_Click
õõ 6
(
õõ6 7
object
õõ7 =
sender
õõ> D
,
õõD E
RoutedEventArgs
õõF U
e
õõV W
)
õõW X
{
úú 	
if
ùù 
(
ùù 
!
ùù 
string
ùù 
.
ùù  
IsNullOrWhiteSpace
ùù *
(
ùù* +
ChatTextBox
ùù+ 6
.
ùù6 7
Text
ùù7 ;
)
ùù; <
)
ùù< =
{
ûû 
try
üü 
{
†† 
await
°°  
GameServiceManager
°° ,
.
°°, -
Instance
°°- 5
.
°°5 6
Client
°°6 <
.
°°< ="
SendChatMessageAsync
°°= Q
(
°°Q R
ChatTextBox
°°R ]
.
°°] ^
Text
°°^ b
)
°°b c
;
°°c d
ChatTextBox
¢¢ 
.
¢¢  
Text
¢¢  $
=
¢¢% &
string
¢¢' -
.
¢¢- .
Empty
¢¢. 3
;
¢¢3 4
}
££ 
catch
§§ 
(
§§ 
	Exception
§§  
ex
§§! #
)
§§# $
{
•• 
Debug
¶¶ 
.
¶¶ 
	WriteLine
¶¶ #
(
¶¶# $
$"
¶¶$ &
$str
¶¶& 2
{
¶¶2 3
ex
¶¶3 5
.
¶¶5 6
Message
¶¶6 =
}
¶¶= >
"
¶¶> ?
)
¶¶? @
;
¶¶@ A
}
ßß 
}
®® 
}
©© 	
private
´´ 
async
´´ 
void
´´ $
ButtonBackToMenu_Click
´´ 1
(
´´1 2
object
´´2 8
sender
´´9 ?
,
´´? @
RoutedEventArgs
´´A P
e
´´Q R
)
´´R S
{
¨¨ 	
var
≠≠ 
confirmationBox
≠≠ 
=
≠≠  !
new
≠≠" %$
ConfirmationMessageBox
≠≠& <
(
≠≠< =
Lang
ÆÆ 
.
ÆÆ %
Global_Title_LeaveLobby
ÆÆ ,
,
ÆÆ, -
Lang
ÆÆ. 2
.
ÆÆ2 3%
Global_Message_ExitGame
ÆÆ3 J
,
ÆÆJ K
this
ØØ 
,
ØØ $
ConfirmationMessageBox
ØØ ,
.
ØØ, -!
ConfirmationBoxType
ØØ- @
.
ØØ@ A
Warning
ØØA H
)
ØØH I
;
ØØI J
if
±± 
(
±± 
confirmationBox
±± 
.
±±  

ShowDialog
±±  *
(
±±* +
)
±±+ ,
==
±±- /
true
±±0 4
)
±±4 5
{
≤≤ 
await
≥≥ 
LeaveGameSafe
≥≥ #
(
≥≥# $
)
≥≥$ %
;
≥≥% &
}
¥¥ 
}
µµ 	
private
∑∑ 
void
∑∑ "
ButtonSettings_Click
∑∑ )
(
∑∑) *
object
∑∑* 0
sender
∑∑1 7
,
∑∑7 8
RoutedEventArgs
∑∑9 H
e
∑∑I J
)
∑∑J K
{
∏∏ 	
NavigationHelper
ππ 
.
ππ 

ShowDialog
ππ '
(
ππ' (
this
ππ( ,
,
ππ, -
new
ππ. 1
Settings
ππ2 :
(
ππ: ;
)
ππ; <
)
ππ< =
;
ππ= >
}
∫∫ 	
private
¿¿ 
async
¿¿ 
void
¿¿ 
VoteKick_Click
¿¿ )
(
¿¿) *
object
¿¿* 0
sender
¿¿1 7
,
¿¿7 8
RoutedEventArgs
¿¿9 H
e
¿¿I J
)
¿¿J K
{
¡¡ 	
if
¬¬ 
(
¬¬ 
sender
¬¬ 
is
¬¬ 
MenuItem
¬¬ "
menuItem
¬¬# +
&&
¬¬, .
menuItem
√√ 
.
√√ 
Parent
√√ 
is
√√  "
ContextMenu
√√# .
contextMenu
√√/ :
&&
√√; =
contextMenu
ƒƒ 
.
ƒƒ 
PlacementTarget
ƒƒ +
is
ƒƒ, .
Border
ƒƒ/ 5
targetBorder
ƒƒ6 B
&&
ƒƒC E
targetBorder
≈≈ 
.
≈≈ 
Tag
≈≈  
is
≈≈! #
string
≈≈$ *
targetPlayerName
≈≈+ ;
)
≈≈; <
{
∆∆ 
if
«« 
(
«« 
targetPlayerName
«« $
==
««% '
UserSession
««( 3
.
««3 4
Username
««4 <
)
««< =
{
»» 
new
…… 
CustomMessageBox
…… (
(
……( )
Lang
……) -
.
……- ."
Global_Title_Warning
……. B
,
……B C
$str
……D _
,
……_ `
this
……a e
,
……e f
MessageBoxType
……g u
.
……u v
Warning
……v }
)
……} ~
.
……~ 

ShowDialog…… â
(……â ä
)……ä ã
;……ã å
return
   
;
   
}
ÀÀ 
var
ÕÕ 
confirmation
ÕÕ  
=
ÕÕ! "
new
ÕÕ# &$
ConfirmationMessageBox
ÕÕ' =
(
ÕÕ= >
$"
ÕÕ> @
$str
ÕÕ@ M
{
ÕÕM N
targetPlayerName
ÕÕN ^
}
ÕÕ^ _
$str
ÕÕ_ `
"
ÕÕ` a
,
ÕÕa b
$str
ÕÕc q
,
ÕÕq r
this
ÕÕs w
,
ÕÕw x%
ConfirmationMessageBoxÕÕy è
.ÕÕè ê#
ConfirmationBoxTypeÕÕê £
.ÕÕ£ §
QuestionÕÕ§ ¨
)ÕÕ¨ ≠
;ÕÕ≠ Æ
if
œœ 
(
œœ 
confirmation
œœ  
.
œœ  !

ShowDialog
œœ! +
(
œœ+ ,
)
œœ, -
==
œœ. 0
true
œœ1 5
)
œœ5 6
{
–– 
try
—— 
{
““ 
await
””  
GameServiceManager
”” 0
.
””0 1
Instance
””1 9
.
””9 :
Client
””: @
.
””@ A
VoteToKickAsync
””A P
(
””P Q
targetPlayerName
””Q a
)
””a b
;
””b c
}
‘‘ 
catch
’’ 
(
’’ 
	Exception
’’ $
ex
’’% '
)
’’' (
{
÷÷ 
Debug
◊◊ 
.
◊◊ 
	WriteLine
◊◊ '
(
◊◊' (
$"
◊◊( *
$str
◊◊* =
{
◊◊= >
ex
◊◊> @
.
◊◊@ A
Message
◊◊A H
}
◊◊H I
"
◊◊I J
)
◊◊J K
;
◊◊K L
ExceptionManager
ÿÿ (
.
ÿÿ( )
Handle
ÿÿ) /
(
ÿÿ/ 0
ex
ÿÿ0 2
,
ÿÿ2 3
this
ÿÿ4 8
)
ÿÿ8 9
;
ÿÿ9 :
}
ŸŸ 
}
⁄⁄ 
}
€€ 
}
‹‹ 	
private
ﬁﬁ 
void
ﬁﬁ 
ShowMatchSummary
ﬁﬁ %
(
ﬁﬁ% &
string
ﬁﬁ& ,
title
ﬁﬁ- 2
,
ﬁﬁ2 3
string
ﬁﬁ4 :
stats
ﬁﬁ; @
)
ﬁﬁ@ A
{
ﬂﬂ 	
var
‡‡ 
summary
‡‡ 
=
‡‡ 
new
‡‡ 
MatchSummary
‡‡ *
(
‡‡* +
title
‡‡+ 0
,
‡‡0 1
stats
‡‡2 7
)
‡‡7 8
;
‡‡8 9
NavigationHelper
·· 
.
·· 

ShowDialog
·· '
(
··' (
this
··( ,
,
··, -
summary
··. 5
)
··5 6
;
··6 7
_
„„ 
=
„„ 
LeaveGameSafe
„„ 
(
„„ 
)
„„ 
;
„„  
}
‰‰ 	
private
ÊÊ 
async
ÊÊ 
Task
ÊÊ 
LeaveGameSafe
ÊÊ (
(
ÊÊ( )
)
ÊÊ) *
{
ÁÁ 	
UnsubscribeEvents
ËË 
(
ËË 
)
ËË 
;
ËË  
_gameManager
ÈÈ 
.
ÈÈ 
StopGame
ÈÈ !
(
ÈÈ! "
)
ÈÈ" #
;
ÈÈ# $
try
ÎÎ 
{
ÏÏ 
await
ÌÌ  
GameServiceManager
ÌÌ (
.
ÌÌ( )
Instance
ÌÌ) 1
.
ÌÌ1 2
Client
ÌÌ2 8
.
ÌÌ8 9
LeaveLobbyAsync
ÌÌ9 H
(
ÌÌH I
)
ÌÌI J
;
ÌÌJ K
}
ÓÓ 
catch
ÔÔ 
(
ÔÔ 
	Exception
ÔÔ 
ex
ÔÔ 
)
ÔÔ  
{
 
Debug
ÒÒ 
.
ÒÒ 
	WriteLine
ÒÒ 
(
ÒÒ  
$"
ÒÒ  "
$str
ÒÒ" 6
{
ÒÒ6 7
ex
ÒÒ7 9
.
ÒÒ9 :
Message
ÒÒ: A
}
ÒÒA B
"
ÒÒB C
)
ÒÒC D
;
ÒÒD E
}
ÚÚ 
finally
ÛÛ 
{
ÙÙ 
NavigationHelper
ıı  
.
ıı  !

NavigateTo
ıı! +
(
ıı+ ,
this
ıı, 0
,
ıı0 1
new
ıı2 5
MultiplayerMenu
ıı6 E
(
ııE F
)
ııF G
)
ııG H
;
ııH I
}
ˆˆ 
}
˜˜ 	
	protected
˘˘ 
override
˘˘ 
async
˘˘  
void
˘˘! %
OnClosed
˘˘& .
(
˘˘. /
	EventArgs
˘˘/ 8
e
˘˘9 :
)
˘˘: ;
{
˙˙ 	
UnsubscribeEvents
˚˚ 
(
˚˚ 
)
˚˚ 
;
˚˚  
_gameManager
¸¸ 
.
¸¸ 
StopGame
¸¸ !
(
¸¸! "
)
¸¸" #
;
¸¸# $
if
˛˛ 
(
˛˛  
GameServiceManager
˛˛ "
.
˛˛" #
Instance
˛˛# +
.
˛˛+ ,
Client
˛˛, 2
.
˛˛2 3
State
˛˛3 8
==
˛˛9 ;
System
˛˛< B
.
˛˛B C
ServiceModel
˛˛C O
.
˛˛O P 
CommunicationState
˛˛P b
.
˛˛b c
Opened
˛˛c i
)
˛˛i j
{
ˇˇ 
try
ÄÄ 
{
ÅÅ 
await
ÇÇ  
GameServiceManager
ÇÇ ,
.
ÇÇ, -
Instance
ÇÇ- 5
.
ÇÇ5 6
Client
ÇÇ6 <
.
ÇÇ< =
LeaveLobbyAsync
ÇÇ= L
(
ÇÇL M
)
ÇÇM N
;
ÇÇN O
}
ÉÉ 
catch
ÑÑ 
(
ÑÑ 
	Exception
ÑÑ  
ex
ÑÑ! #
)
ÑÑ# $
{
ÖÖ 
Debug
ÜÜ 
.
ÜÜ 
	WriteLine
ÜÜ #
(
ÜÜ# $
$"
ÜÜ$ &
$str
ÜÜ& Q
{
ÜÜQ R
ex
ÜÜR T
.
ÜÜT U
Message
ÜÜU \
}
ÜÜ\ ]
"
ÜÜ] ^
)
ÜÜ^ _
;
ÜÜ_ `
}
áá 
}
àà 
try
ää 
{
ãã 
if
åå 
(
åå 
this
åå 
.
åå 
Owner
åå 
!=
åå !
null
åå" &
&&
åå' )
Application
åå* 5
.
åå5 6
Current
åå6 =
.
åå= >

MainWindow
åå> H
!=
ååI K
this
ååL P
.
ååP Q
Owner
ååQ V
)
ååV W
{
çç 
this
éé 
.
éé 
Owner
éé 
.
éé 
Show
éé #
(
éé# $
)
éé$ %
;
éé% &
}
èè 
}
êê 
catch
ëë 
(
ëë '
InvalidOperationException
ëë ,
ex
ëë- /
)
ëë/ 0
{
íí 
Debug
ìì 
.
ìì 
	WriteLine
ìì 
(
ìì  
$"
ìì  "
$str
ìì" N
{
ììN O
ex
ììO Q
.
ììQ R
Message
ììR Y
}
ììY Z
"
ììZ [
)
ìì[ \
;
ìì\ ]
}
îî 
base
ññ 
.
ññ 
OnClosed
ññ 
(
ññ 
e
ññ 
)
ññ 
;
ññ 
}
óó 	
}
öö 
}õõ ë5
AC:\MemoryGame\Client\Client\Views\Session\RegisterAccount.xaml.cs
	namespace 	
Client
 
. 
Views 
. 
Session 
{ 
public 

partial 
class 
RegisterAccount (
:) *
Window+ 1
{ 
private 
readonly 
bool 
_isGuestRegister .
;. /
public 
RegisterAccount 
( 
bool #
isGuestRegister$ 3
=4 5
false6 ;
); <
{ 	
InitializeComponent 
(  
)  !
;! "
_isGuestRegister 
= 
isGuestRegister .
;. /
if 
( 
_isGuestRegister  
)  !
{ 
TitleRegister 
. 
Content %
=& '
Lang( ,
., --
!RegisterAccount_Title_LinkAccount- N
;N O
} 
} 	
private 
async 
void -
!ButtonAcceptRegisterAccount_Click <
(< =
object= C
senderD J
,J K
RoutedEventArgsL [
e\ ]
)] ^
{   	
string!! 
email!! 
=!! 
TextBoxEmail!! '
.!!' (
Text!!( ,
.!!, -
Trim!!- 1
(!!1 2
)!!2 3
;!!3 4
string"" 
password"" 
="" 
TextBoxPassword"" -
.""- .
Text"". 2
?""2 3
.""3 4
Trim""4 8
(""8 9
)""9 :
;"": ;
LabelEmailError$$ 
.$$ 
Content$$ #
=$$$ %
$str$$& (
;$$( )
LabelPasswordError%% 
.%% 
Content%% &
=%%' (
$str%%) +
;%%+ ,
ValidationCode'' 
validationEmail'' *
=''+ ,
ValidateEmail''- :
('': ;
email''; @
)''@ A
;''A B
if(( 
((( 
validationEmail(( 
!=((  "
ValidationCode((# 1
.((1 2
Success((2 9
)((9 :
{)) 
LabelEmailError** 
.**  
Content**  '
=**( )
	GetString*** 3
(**3 4
validationEmail**4 C
)**C D
;**D E
return++ 
;++ 
},, 
ValidationCode.. 
validationPassword.. -
=... /
ValidatePassword..0 @
(..@ A
password..A I
)..I J
;..J K
if// 
(// 
validationPassword// "
!=//# %
ValidationCode//& 4
.//4 5
Success//5 <
)//< =
{00 
LabelPasswordError11 "
.11" #
Content11# *
=11+ ,
	GetString11- 6
(116 7
validationPassword117 I
)11I J
;11J K
return22 
;22 
}33 '
ButtonAcceptRegisterAccount55 '
.55' (
	IsEnabled55( 1
=552 3
false554 9
;559 :
try77 
{88 
ResponseDTO99 
response99 $
;99$ %
if;; 
(;; 
_isGuestRegister;; $
);;$ %
{<< 
response== 
=== 
await== $
UserServiceManager==% 7
.==7 8
Instance==8 @
.==@ A
Client==A G
.==G H*
InitiateGuestRegistrationAsync==H f
(==f g
UserSession>> #
.>># $
UserId>>$ *
,>>* +
email>>, 1
,>>1 2
password>>3 ;
)>>; <
;>>< =
}?? 
else@@ 
{AA 
responseBB 
=BB 
awaitBB $
UserServiceManagerBB% 7
.BB7 8
InstanceBB8 @
.BB@ A
ClientBBA G
.BBG H"
StartRegistrationAsyncBBH ^
(BB^ _
emailBB_ d
,BBd e
passwordBBf n
)BBn o
;BBo p
}CC 
ifEE 
(EE 
responseEE 
.EE 
SuccessEE $
)EE$ %
{FF 
newGG 
CustomMessageBoxGG (
(GG( )
LangHH 
.HH  
Global_Title_SuccessHH 1
,HH1 2
LangHH3 7
.HH7 8+
RegisterAccount_Message_SuccessHH8 W
,HHW X
thisII 
,II 
MessageBoxTypeII ,
.II, -
SuccessII- 4
)II4 5
.II5 6

ShowDialogII6 @
(II@ A
)IIA B
;IIB C
NavigationHelperKK $
.KK$ %

NavigateToKK% /
(KK/ 0
thisKK0 4
,KK4 5
newKK6 9

VerifyCodeKK: D
(KKD E
emailKKE J
,KKJ K
_isGuestRegisterKKL \
)KK\ ]
)KK] ^
;KK^ _
}LL 
elseMM 
{NN 
stringOO 
errorMessageOO '
=OO( )
	GetStringOO* 3
(OO3 4
responseOO4 <
.OO< =

MessageKeyOO= G
)OOG H
;OOH I
newQQ 
CustomMessageBoxQQ (
(QQ( )
LangRR 
.RR 
Global_Title_ErrorRR /
,RR/ 0
errorMessageRR1 =
,RR= >
thisSS 
,SS 
MessageBoxTypeSS ,
.SS, -
ErrorSS- 2
)SS2 3
.SS3 4

ShowDialogSS4 >
(SS> ?
)SS? @
;SS@ A'
ButtonAcceptRegisterAccountUU /
.UU/ 0
	IsEnabledUU0 9
=UU: ;
trueUU< @
;UU@ A
}VV 
}WW 
catchXX 
(XX 
	ExceptionXX 
exXX 
)XX  
{YY 
ExceptionManagerZZ  
.ZZ  !
HandleZZ! '
(ZZ' (
exZZ( *
,ZZ* +
thisZZ, 0
,ZZ0 1
(ZZ2 3
)ZZ3 4
=>ZZ5 7'
ButtonAcceptRegisterAccountZZ8 S
.ZZS T
	IsEnabledZZT ]
=ZZ^ _
trueZZ` d
)ZZd e
;ZZe f
}[[ 
}\\ 	
private^^ 
void^^ )
ButtonBackToTitleScreen_Click^^ 2
(^^2 3
object^^3 9
sender^^: @
,^^@ A
RoutedEventArgs^^B Q
e^^R S
)^^S T
{__ 	
NavigationHelper`` 
.`` 

NavigateTo`` '
(``' (
this``( ,
,``, -
this``. 2
.``2 3
Owner``3 8
??``9 ;
new``< ?
TitleScreen``@ K
(``K L
)``L M
)``M N
;``N O
}aa 	
}bb 
}cc †+
DC:\MemoryGame\Client\Client\Views\Session\EnterUsernameGuest.xaml.cs
	namespace 	
Client
 
. 
Views 
. 
Session 
{ 
public 

partial 
class 
EnterUsernameGuest +
:, -
Window. 4
{ 
public 
EnterUsernameGuest !
(! "
)" #
{ 	
InitializeComponent 
(  
)  !
;! "
} 	
private 
void "
TextBox_PreviewKeyDown +
(+ ,
object, 2
sender3 9
,9 :
KeyEventArgs; G
eH I
)I J
{ 	
if 
( 
e 
. 
Key 
== 
Key 
. 
Space "
)" #
{ 
e 
. 
Handled 
= 
true  
;  !
} 
} 	
private!! 
async!! 
void!! +
ButtonAcceptUsernameGuest_Click!! :
(!!: ;
object!!; A
sender!!B H
,!!H I
RoutedEventArgs!!J Y
e!!Z [
)!![ \
{"" 	
string## 
username## 
=## 
TextBoxUsername## -
.##- .
Text##. 2
.##2 3
Trim##3 7
(##7 8
)##8 9
;##9 :
LabelUsernameError$$ 
.$$ 
Content$$ &
=$$' (
$str$$) +
;$$+ ,
ValidationCode&& 
validationUsername&& -
=&&. /
ValidateUsername&&0 @
(&&@ A
username&&A I
)&&I J
;&&J K
if'' 
('' 
validationUsername'' "
!=''# %
ValidationCode''& 4
.''4 5
Success''5 <
)''< =
{(( 
LabelUsernameError)) "
.))" #
Content))# *
=))+ ,
	GetString))- 6
())6 7
validationUsername))7 I
)))I J
;))J K
return** 
;** 
}++ %
ButtonAcceptUsernameGuest-- %
.--% &
	IsEnabled--& /
=--0 1
false--2 7
;--7 8
try// 
{00 
LoginResponse11 
response11 &
=11' (
await11) .
UserServiceManager11/ A
.11A B
Instance11B J
.11J K
Client11K Q
.11Q R
LoginAsGuestAsync11R c
(11c d
username11d l
)11l m
;11m n
if33 
(33 
response33 
.33 
Success33 $
)33$ %
{44 
UserSession55 
.55  
StartSession55  ,
(55, -
response55- 5
.555 6
SessionToken556 B
,55B C
response55D L
.55L M
User55M Q
)55Q R
;55R S
new77 
CustomMessageBox77 (
(77( )
Lang88 
.88 ,
 Global_Title_LoginAsGuestSuccess88 =
,88= >
string99 
.99 
Format99 %
(99% &
Lang99& *
.99* +"
Global_Message_Welcome99+ A
,99A B
response99C K
.99K L
User99L P
.99P Q
Username99Q Y
)99Y Z
,99Z [
this:: 
,:: 
MessageBoxType:: ,
.::, -
Success::- 4
)::4 5
.::5 6

ShowDialog::6 @
(::@ A
)::A B
;::B C
NavigationHelper<< $
.<<$ %

NavigateTo<<% /
(<</ 0
this<<0 4
,<<4 5
new<<6 9
MainMenu<<: B
(<<B C
)<<C D
)<<D E
;<<E F
}== 
else>> 
{?? 
new@@ 
CustomMessageBox@@ (
(@@( )
LangAA 
.AA 
Global_Title_ErrorAA /
,AA/ 0
	GetStringAA1 :
(AA: ;
responseAA; C
.AAC D

MessageKeyAAD N
)AAN O
,AAO P
thisBB 
,BB 
MessageBoxTypeBB ,
.BB, -
ErrorBB- 2
)BB2 3
.BB3 4

ShowDialogBB4 >
(BB> ?
)BB? @
;BB@ A%
ButtonAcceptUsernameGuestDD -
.DD- .
	IsEnabledDD. 7
=DD8 9
trueDD: >
;DD> ?
}EE 
}FF 
catchGG 
(GG 
	ExceptionGG 
exGG 
)GG  
{HH 
ExceptionManagerII  
.II  !
HandleII! '
(II' (
exII( *
,II* +
thisII, 0
,II0 1
(II2 3
)II3 4
=>II5 7%
ButtonAcceptUsernameGuestII8 Q
.IIQ R
	IsEnabledIIR [
=II\ ]
trueII^ b
)IIb c
;IIc d
}JJ 
}KK 	
privateMM 
voidMM )
ButtonBackToTitleScreen_ClickMM 2
(MM2 3
objectMM3 9
senderMM: @
,MM@ A
RoutedEventArgsMMB Q
eMMR S
)MMS T
{NN 	
NavigationHelperOO 
.OO 

NavigateToOO '
(OO' (
thisOO( ,
,OO, -
thisOO. 2
.OO2 3
OwnerOO3 8
??OO9 ;
newOO< ?
TitleScreenOO@ K
(OOK L
)OOL M
)OOM N
;OON O
}PP 	
}QQ 
}RR ›
>C:\MemoryGame\Client\Client\Views\Profile\StatsHistory.xaml.cs
	namespace

 	
Client


 
.

 
Views

 
.

 
Profile

 
{ 
public 

partial 
class 
StatsHistory %
:& '
Window( .
{ 
public 
StatsHistory 
( 
) 
{ 	
InitializeComponent 
(  
)  !
;! "
_ 
= 
LoadMatchHistory  
(  !
)! "
;" #
} 	
private 
async 
Task 
LoadMatchHistory +
(+ ,
), -
{ 	
try 
{ 
var 
history 
= 
await #
UserServiceManager$ 6
.6 7
Instance7 ?
.? @
Client@ F
.F G 
GetMatchHistoryAsyncG [
([ \
UserSession\ g
.g h
SessionTokenh t
)t u
;u v
DataGridHistory 
.  
ItemsSource  +
=, -
history. 5
;5 6
} 
catch 
( 
	Exception 
ex 
)  
{ 
ExceptionManager    
.    !
Handle  ! '
(  ' (
ex  ( *
,  * +
this  , 0
)  0 1
;  1 2
}!! 
}"" 	
private$$ 
void$$ 
ButtonBack_Click$$ %
($$% &
object$$& ,
sender$$- 3
,$$3 4
RoutedEventArgs$$5 D
e$$E F
)$$F G
{%% 	
NavigationHelper&& 
.&& 

NavigateTo&& '
(&&' (
this&&( ,
,&&, -
this&&. 2
.&&2 3
Owner&&3 8
??&&9 ;
new&&< ?
PlayerProfile&&@ M
(&&M N
)&&N O
)&&O P
;&&P Q
}'' 	
private)) 
void)) 
ButtonReport_Click)) '
())' (
object))( .
sender))/ 5
,))5 6
RoutedEventArgs))7 F
e))G H
)))H I
{** 	
if++ 
(++ 
sender++ 
is++ 
Button++  
button++! '
&&++( *
button+++ 1
.++1 2
Tag++2 5
is++6 8
MatchHistoryDTO++9 H
	matchInfo++I R
)++R S
{,, 
string-- 
userToReport-- #
=--$ %
	matchInfo--& /
.--/ 0

WinnerName--0 :
;--: ;
var.. 
reportDialog..  
=..! "
new..# &
ReportUserDialog..' 7
(..7 8
userToReport..8 D
,..D E
	matchInfo..F O
...O P
MatchId..P W
)..W X
;..X Y
NavigationHelper//  
.//  !

ShowDialog//! +
(//+ ,
this//, 0
,//0 1
reportDialog//2 >
)//> ?
;//? @
}00 
}11 	
}22 
}33 ìE
?C:\MemoryGame\Client\Client\Views\Profile\PlayerProfile.xaml.cs
	namespace		 	
Client		
 
.		 
Views		 
.		 
Profile		 
{

 
public 

partial 
class 
PlayerProfile &
:' (
Window) /
{ 
public 
PlayerProfile 
( 
) 
{ 	
InitializeComponent 
(  
)  !
;! "
_ 
= 
LoadCurrentAvatar !
(! "
)" #
;# $
LoadData 
( 
) 
; 
UserSession 
. 
ProfileUpdated &
+=' )
LoadData* 2
;2 3
} 	
private 
async 
Task 
LoadCurrentAvatar ,
(, -
)- .
{ 	
try 
{ 
var 
bytes 
= 
await !
UserServiceManager" 4
.4 5
Instance5 =
.= >
Client> D
.D E
GetUserAvatarAsyncE W
(W X
UserSessionX c
.c d
Emaild i
)i j
;j k
if 
( 
bytes 
!= 
null !
&&" $
bytes% *
.* +
Length+ 1
>2 3
$num4 5
)5 6
{ 
ImageAvatar 
.  
Source  &
=' (
ImageHelper) 4
.4 5"
ByteArrayToImageSource5 K
(K L
bytesL Q
)Q R
;R S
}   
}!! 
catch"" 
("" 
	Exception"" 
ex"" 
)""  
{## 
ExceptionManager$$  
.$$  !
Handle$$! '
($$' (
ex$$( *
,$$* +
this$$, 0
)$$0 1
;$$1 2
}%% 
}&& 	
private(( 
void(( 
LoadData(( 
((( 
)(( 
{)) 	
string** 
fullName** 
=** 
$"**  
{**  !
UserSession**! ,
.**, -
Name**- 1
}**1 2
$str**2 3
{**3 4
UserSession**4 ?
.**? @
LastName**@ H
}**H I
"**I J
.**J K
Trim**K O
(**O P
)**P Q
;**Q R
string++ 

formatDate++ 
=++ 
$str++  .
;++. /
if-- 
(-- 
string-- 
.-- 
IsNullOrEmpty-- $
(--$ %
fullName--% -
)--- .
)--. /
{.. 
TextBlockFullName// !
.//! "
Text//" &
=//' (
Lang//) -
.//- .#
Global_Label_NoRegister//. E
;//E F
}00 
else11 
{22 
TextBlockFullName33 !
.33! "
Text33" &
=33' (
fullName33) 1
;331 2
}44 
TextBlockUsername66 
.66 
Text66 "
=66# $
UserSession66% 0
.660 1
Username661 9
;669 :
	TextEmail77 
.77 
Text77 
=77 
UserSession77 (
.77( )
Email77) .
;77. /
TextDate88 
.88 
Text88 
=88 
UserSession88 '
.88' (
RegistrationDate88( 8
.888 9
ToString889 A
(88A B

formatDate88B L
)88L M
;88M N&
ItemsControlSocialNetworks99 &
.99& '
ItemsSource99' 2
=993 4
UserSession995 @
.99@ A
SocialNetworks99A O
;99O P
}:: 	
private<< 
void<< #
ButtonEditProfile_Click<< ,
(<<, -
object<<- 3
sender<<4 :
,<<: ;
RoutedEventArgs<<< K
e<<L M
)<<M N
{== 	
NavigationHelper>> 
.>> 

NavigateTo>> '
(>>' (
this>>( ,
,>>, -
new>>. 1
EditProfile>>2 =
(>>= >
)>>> ?
)>>? @
;>>@ A
}?? 	
privateAA 
voidAA $
ButtonCloseSession_ClickAA -
(AA- .
objectAA. 4
senderAA5 ;
,AA; <
RoutedEventArgsAA= L
eAAM N
)AAN O
{BB 	
varCC 
confirmationBoxCC 
=CC  !
newCC" %"
ConfirmationMessageBoxCC& <
(CC< =
LangDD 
.DD  
Global_Button_LogOutDD )
,DD) *
LangDD+ /
.DD/ 0'
Global_Message_CloseSessionDD0 K
,DDK L
thisEE 
,EE "
ConfirmationMessageBoxEE ,
.EE, -
ConfirmationBoxTypeEE- @
.EE@ A
CriticEEA G
)EEG H
;EEH I
ifGG 
(GG 
confirmationBoxGG 
.GG  

ShowDialogGG  *
(GG* +
)GG+ ,
==GG- /
trueGG0 4
)GG4 5
{HH 
tryII 
{JJ 
ifKK 
(KK 
UserSessionKK #
.KK# $
IsGuestKK$ +
)KK+ ,
{LL 
UserServiceManagerMM *
.MM* +
InstanceMM+ 3
.MM3 4
ClientMM4 :
.MM: ;
LogoutGuestAsyncMM; K
(MMK L
UserSessionMML W
.MMW X
SessionTokenMMX d
)MMd e
;MMe f
}NN 
elseOO 
{PP 
UserServiceManagerQQ *
.QQ* +
InstanceQQ+ 3
.QQ3 4
ClientQQ4 :
.QQ: ;
LogoutAsyncQQ; F
(QQF G
UserSessionQQG R
.QQR S
SessionTokenQQS _
)QQ_ `
;QQ` a
}RR 
}SS 
catchTT 
(TT 
	ExceptionTT  
exTT! #
)TT# $
{UU 
SystemVV 
.VV 
DiagnosticsVV &
.VV& '
DebugVV' ,
.VV, -
	WriteLineVV- 6
(VV6 7
$"VV7 9
$strVV9 I
{VVI J
exVVJ L
}VVL M
"VVM N
)VVN O
;VVO P
}WW 
UserSessionYY 
.YY 

EndSessionYY &
(YY& '
)YY' (
;YY( )
NavigationHelperZZ  
.ZZ  !

NavigateToZZ! +
(ZZ+ ,
thisZZ, 0
,ZZ0 1
newZZ2 5
TitleScreenZZ6 A
(ZZA B
)ZZB C
)ZZC D
;ZZD E
}[[ 
}\\ 	
private^^ 
void^^ 
ButtonFriends_Click^^ (
(^^( )
object^^) /
sender^^0 6
,^^6 7
RoutedEventArgs^^8 G
e^^H I
)^^I J
{__ 	
NavigationHelper`` 
.`` 

NavigateTo`` '
(``' (
this``( ,
,``, -
new``. 1
Social``2 8
.``8 9
FriendsMenu``9 D
(``D E
)``E F
)``F G
;``G H
}aa 	
privatecc 
voidcc 
ButtonStats_Clickcc &
(cc& '
objectcc' -
sendercc. 4
,cc4 5
RoutedEventArgscc6 E
eccF G
)ccG H
{dd 	
NavigationHelperee 
.ee 

NavigateToee '
(ee' (
thisee( ,
,ee, -
newee. 1
StatsHistoryee2 >
(ee> ?
)ee? @
)ee@ A
;eeA B
}ff 	
privatehh 
voidhh 
ButtonBack_Clickhh %
(hh% &
objecthh& ,
senderhh- 3
,hh3 4
RoutedEventArgshh5 D
ehhE F
)hhF G
{ii 	
NavigationHelperjj 
.jj 

NavigateTojj '
(jj' (
thisjj( ,
,jj, -
thisjj. 2
.jj2 3
Ownerjj3 8
??jj9 ;
newjj< ?
MainMenujj@ H
(jjH I
)jjI J
)jjJ K
;jjK L
}kk 	
privatemm 
voidmm 
OnProfileUpdatedmm %
(mm% &
)mm& '
{nn 	
_oo 
=oo 
LoadCurrentAvataroo !
(oo! "
)oo" #
;oo# $
}pp 	
	protectedrr 
overriderr 
voidrr 
OnClosedrr  (
(rr( )
	EventArgsrr) 2
err3 4
)rr4 5
{ss 	
UserSessiontt 
.tt 
ProfileUpdatedtt &
-=tt' )
OnProfileUpdatedtt* :
;tt: ;
baseuu 
.uu 
OnCloseduu 
(uu 
euu 
)uu 
;uu 
}vv 	
}ww 
}xx À¯
=C:\MemoryGame\Client\Client\Views\Profile\EditProfile.xaml.cs
	namespace 	
Client
 
. 
Views 
. 
Profile 
{ 
public 

partial 
class 
EditProfile $
:% &
Window' -
{ 
public  
ObservableCollection #
<# $
SocialNetworkDTO$ 4
>4 5
SocialNetworksList6 H
{I J
getK N
;N O
setP S
;S T
}U V
public 
EditProfile 
( 
) 
{ 	
InitializeComponent 
(  
)  !
;! "
SocialNetworksList 
=  
new! $ 
ObservableCollection% 9
<9 :
SocialNetworkDTO: J
>J K
(K L
)L M
;M N
ItemsControlSocials 
.  
ItemsSource  +
=, -
SocialNetworksList. @
;@ A
LoadExistingData   
(   
)   
;   
_!! 
=!! 
InitializeAsync!! 
(!!  
)!!  !
;!!! "
}"" 	
private$$ 
async$$ 
Task$$ 
InitializeAsync$$ *
($$* +
)$$+ ,
{%% 	
await&& 
LoadCurrentAvatar&& #
(&&# $
)&&$ %
;&&% &
}'' 	
private)) 
void)) 
LoadExistingData)) %
())% &
)))& '
{** 	
if++ 
(++ 
FindName++ 
(++ 
$str++ -
)++- .
is++/ 1
TextBox++2 9
txtUser++: A
)++A B
{,, 
txtUser-- 
.-- 
Text-- 
=-- 
UserSession-- *
.--* +
Username--+ 3
;--3 4
}.. 
if00 
(00 
FindName00 
(00 
$str00 &
)00& '
is00( *
TextBox00+ 2
txtName003 :
)00: ;
{11 
txtName22 
.22 
Text22 
=22 
UserSession22 *
.22* +
Name22+ /
??220 2
$str223 5
;225 6
}33 
if44 
(44 
FindName44 
(44 
$str44 *
)44* +
is44, .
TextBox44/ 6
txtLast447 >
)44> ?
{55 
txtLast66 
.66 
Text66 
=66 
UserSession66 *
.66* +
LastName66+ 3
??664 6
$str667 9
;669 :
}77 
if99 
(99 
UserSession99 
.99 
SocialNetworks99 *
!=99+ -
null99. 2
)992 3
{:: 
foreach;; 
(;; 
var;; 
social;; #
in;;$ &
UserSession;;' 2
.;;2 3
SocialNetworks;;3 A
);;A B
{<< 
SocialNetworksList== &
.==& '
Add==' *
(==* +
social==+ 1
)==1 2
;==2 3
}>> 
}?? 
}@@ 	
privateBB 
asyncBB 
TaskBB 
LoadCurrentAvatarBB ,
(BB, -
)BB- .
{CC 	
tryDD 
{EE 
varFF 
bytesFF 
=FF 
awaitFF !
UserServiceManagerFF" 4
.FF4 5
InstanceFF5 =
.FF= >
ClientFF> D
.FFD E
GetUserAvatarAsyncFFE W
(FFW X
UserSessionFFX c
.FFc d
EmailFFd i
)FFi j
;FFj k
ifGG 
(GG 
bytesGG 
!=GG 
nullGG !
&&GG" $
bytesGG% *
.GG* +
LengthGG+ 1
>GG2 3
$numGG4 5
)GG5 6
{HH 
ImageAvatarII 
.II  
SourceII  &
=II' (
ImageHelperII) 4
.II4 5"
ByteArrayToImageSourceII5 K
(IIK L
bytesIIL Q
)IIQ R
;IIR S
}JJ 
}KK 
catchLL 
(LL 
	ExceptionLL 
exLL 
)LL  
{MM 
ExceptionManagerNN  
.NN  !
HandleNN! '
(NN' (
exNN( *
,NN* +
thisNN, 0
)NN0 1
;NN1 2
}OO 
}PP 	
privateRR 
asyncRR 
voidRR $
ButtonChangeAvatar_ClickRR 3
(RR3 4
objectRR4 :
senderRR; A
,RRA B
RoutedEventArgsRRC R
eRRS T
)RRT U
{SS 	
varTT 
avatarDialogTT 
=TT 
NavigationHelperTT /
.TT/ 0
GetOpenFileDialogTT0 A
(TTA B
LangUU 
.UU %
SetupProfile_Dialog_TitleUU .
,UU. /
LangVV 
.VV &
SetupProfile_Dialog_FilterVV /
,VV/ 0
falseWW 
)WW 
;WW 
ifYY 
(YY 
avatarDialogYY 
.YY 

ShowDialogYY '
(YY' (
)YY( )
==YY* ,
trueYY- 1
)YY1 2
{ZZ 
try[[ 
{\\ 
byte]] 
[]] 
]]] 
originalBytes]] (
=]]) *
File]]+ /
.]]/ 0
ReadAllBytes]]0 <
(]]< =
avatarDialog]]= I
.]]I J
FileName]]J R
)]]R S
;]]S T
byte^^ 
[^^ 
]^^ 
resizedBytes^^ '
=^^( )
ImageHelper^^* 5
.^^5 6
ResizeImage^^6 A
(^^A B
originalBytes^^B O
,^^O P
$num^^Q T
,^^T U
$num^^V Y
)^^Y Z
;^^Z [
if`` 
(`` 
resizedBytes`` $
.``$ %
Length``% +
==``, .
$num``/ 0
)``0 1
{aa 
	ShowErrorbb !
(bb! "
Langbb" &
.bb& '
Global_Title_Errorbb' 9
,bb9 :
Langbb; ?
.bb? @#
Global_Error_ImageEmptybb@ W
)bbW X
;bbX Y
returncc 
;cc 
}dd 
varff 
responseff  
=ff! "
awaitff# (
UserServiceManagerff) ;
.ff; <
Instanceff< D
.ffD E
ClientffE K
.ffK L!
UpdateUserAvatarAsyncffL a
(ffa b
UserSessionffb m
.ffm n
SessionTokenffn z
,ffz {
resizedBytes	ff| à
)
ffà â
;
ffâ ä
ifhh 
(hh 
responsehh  
.hh  !
Successhh! (
)hh( )
{ii 
ImageAvatarjj #
.jj# $
Sourcejj$ *
=jj+ ,
ImageHelperjj- 8
.jj8 9"
ByteArrayToImageSourcejj9 O
(jjO P
resizedBytesjjP \
)jj\ ]
;jj] ^
UserSessionkk #
.kk# $
OnProfileUpdatedkk$ 4
(kk4 5
)kk5 6
;kk6 7
ShowSuccessll #
(ll# $
Langll$ (
.ll( ) 
Global_Title_Successll) =
,ll= >
Langll? C
.llC D+
EditProfile_Label_AvatarUpdatedllD c
)llc d
;lld e
}mm 
elsenn 
{oo 
	ShowErrorpp !
(pp! "
Langpp" &
.pp& '
Global_Title_Errorpp' 9
,pp9 :
	GetStringpp; D
(ppD E
responseppE M
.ppM N

MessageKeyppN X
)ppX Y
)ppY Z
;ppZ [
}qq 
}rr 
catchss 
(ss %
InvalidOperationExceptionss 0
exss1 3
)ss3 4
whenss5 9
(ss: ;
exss; =
.ss= >
Messagess> E
==ssF H
$strssI X
)ssX Y
{tt 
	ShowErroruu 
(uu 
Languu "
.uu" #
Global_Title_Erroruu# 5
,uu5 6
Languu7 ;
.uu; <&
Global_Error_ImageTooLargeuu< V
)uuV W
;uuW X
}vv 
catchww 
(ww 
	Exceptionww  
exww! #
)ww# $
{xx 
ExceptionManageryy $
.yy$ %
Handleyy% +
(yy+ ,
exyy, .
,yy. /
thisyy0 4
)yy4 5
;yy5 6
}zz 
}{{ 
}|| 	
private~~ 
async~~ 
void~~ &
ButtonUpdatePassword_Click~~ 5
(~~5 6
object~~6 <
sender~~= C
,~~C D
RoutedEventArgs~~E T
e~~U V
)~~V W
{ 	
string
ÄÄ 
currentPass
ÄÄ 
=
ÄÄ   
PasswordBoxCurrent
ÄÄ! 3
.
ÄÄ3 4
Password
ÄÄ4 <
;
ÄÄ< =
string
ÅÅ 
newPass
ÅÅ 
=
ÅÅ 
PasswordBoxNew
ÅÅ +
.
ÅÅ+ ,
Password
ÅÅ, 4
;
ÅÅ4 5
if
ÉÉ 
(
ÉÉ 
string
ÉÉ 
.
ÉÉ  
IsNullOrWhiteSpace
ÉÉ )
(
ÉÉ) *
currentPass
ÉÉ* 5
)
ÉÉ5 6
||
ÉÉ7 9
string
ÉÉ: @
.
ÉÉ@ A 
IsNullOrWhiteSpace
ÉÉA S
(
ÉÉS T
newPass
ÉÉT [
)
ÉÉ[ \
)
ÉÉ\ ]
{
ÑÑ 
new
ÖÖ 
CustomMessageBox
ÖÖ $
(
ÖÖ$ %
Lang
ÖÖ% )
.
ÖÖ) *"
Global_Title_Warning
ÖÖ* >
,
ÖÖ> ?
Lang
ÖÖ@ D
.
ÖÖD E3
%EditProfile_Label_ErrorPasswordFields
ÖÖE j
,
ÖÖj k
this
ÜÜ 
,
ÜÜ 
MessageBoxType
ÜÜ (
.
ÜÜ( )
Warning
ÜÜ) 0
)
ÜÜ0 1
.
ÜÜ1 2

ShowDialog
ÜÜ2 <
(
ÜÜ< =
)
ÜÜ= >
;
ÜÜ> ?
return
áá 
;
áá 
}
àà 
try
ää 
{
ãã 
var
åå 
response
åå 
=
åå 
await
åå $ 
UserServiceManager
åå% 7
.
åå7 8
Instance
åå8 @
.
åå@ A
Client
ååA G
.
ååG H!
ChangePasswordAsync
ååH [
(
åå[ \
UserSession
åå\ g
.
ååg h
SessionToken
ååh t
,
ååt u
currentPassååv Å
,ååÅ Ç
newPassååÉ ä
)ååä ã
;ååã å
if
çç 
(
çç 
response
çç 
.
çç 
Success
çç $
)
çç$ %
{
éé 
ShowSuccess
èè 
(
èè  
Lang
èè  $
.
èè$ %"
Global_Title_Success
èè% 9
,
èè9 :
Lang
èè; ?
.
èè? @/
!EditProfile_Label_PasswordUpdated
èè@ a
)
èèa b
;
èèb c 
PasswordBoxCurrent
êê &
.
êê& '
Password
êê' /
=
êê0 1
$str
êê2 4
;
êê4 5
PasswordBoxNew
ëë "
.
ëë" #
Password
ëë# +
=
ëë, -
$str
ëë. 0
;
ëë0 1
}
íí 
else
ìì 
{
îî 
	ShowError
ïï 
(
ïï 
Lang
ïï "
.
ïï" # 
Global_Title_Error
ïï# 5
,
ïï5 6
Lang
ïï7 ;
.
ïï; <3
%EditProfile_Label_ErrorPasswordUpdate
ïï< a
)
ïïa b
;
ïïb c
}
ññ 
}
óó 
catch
òò 
(
òò 
	Exception
òò 
ex
òò 
)
òò  
{
ôô 
ExceptionManager
öö  
.
öö  !
Handle
öö! '
(
öö' (
ex
öö( *
,
öö* +
this
öö, 0
)
öö0 1
;
öö1 2
}
õõ 
}
úú 	
private
ûû 
async
ûû 
void
ûû (
ButtonUpdateUsername_Click
ûû 5
(
ûû5 6
object
ûû6 <
sender
ûû= C
,
ûûC D
RoutedEventArgs
ûûE T
e
ûûU V
)
ûûV W
{
üü 	
string
†† 
newUsername
†† 
=
††   
TextBoxNewUsername
††! 3
.
††3 4
Text
††4 8
.
††8 9
Trim
††9 =
(
††= >
)
††> ?
;
††? @
if
¢¢ 
(
¢¢ 
string
¢¢ 
.
¢¢ 
IsNullOrEmpty
¢¢ $
(
¢¢$ %
newUsername
¢¢% 0
)
¢¢0 1
)
¢¢1 2
{
££ 
	ShowError
§§ 
(
§§ 
Lang
§§ 
.
§§  
Global_Title_Error
§§ 1
,
§§1 2
Lang
§§3 7
.
§§7 82
$EditProfile_Label_ErrorUsernameEmpty
§§8 \
)
§§\ ]
;
§§] ^
return
•• 
;
•• 
}
¶¶ 
if
®® 
(
®® 
newUsername
®® 
==
®® 
UserSession
®® *
.
®®* +
Username
®®+ 3
)
®®3 4
{
©© 
	ShowError
™™ 
(
™™ 
Lang
™™ 
.
™™  
Global_Title_Error
™™ 1
,
™™1 2
Lang
™™3 7
.
™™7 81
#EditProfile_Label_ErrorSameUsername
™™8 [
)
™™[ \
;
™™\ ]
return
´´ 
;
´´ 
}
¨¨ "
ButtonUpdateUsername
ÆÆ  
.
ÆÆ  !
	IsEnabled
ÆÆ! *
=
ÆÆ+ ,
false
ÆÆ- 2
;
ÆÆ2 3
try
∞∞ 
{
±± 
var
≤≤ 
response
≤≤ 
=
≤≤ 
await
≤≤ $ 
UserServiceManager
≤≤% 7
.
≤≤7 8
Instance
≤≤8 @
.
≤≤@ A
Client
≤≤A G
.
≤≤G H!
ChangeUsernameAsync
≤≤H [
(
≤≤[ \
UserSession
≤≤\ g
.
≤≤g h
SessionToken
≤≤h t
,
≤≤t u
newUsername≤≤v Å
)≤≤Å Ç
;≤≤Ç É
if
¥¥ 
(
¥¥ 
response
¥¥ 
.
¥¥ 
Success
¥¥ $
)
¥¥$ %
{
µµ 
new
∂∂ 
CustomMessageBox
∂∂ (
(
∂∂( )
Lang
∂∂) -
.
∂∂- ."
Global_Title_Success
∂∂. B
,
∂∂B C
Lang
∂∂D H
.
∂∂H I4
&EditProfile_Label_SuccesUpdateUsername
∂∂I o
,
∂∂o p
this
∑∑ 
,
∑∑ 
MessageBoxType
∑∑ ,
.
∑∑, -
Information
∑∑- 8
)
∑∑8 9
.
∑∑9 :

ShowDialog
∑∑: D
(
∑∑D E
)
∑∑E F
;
∑∑F G
try
ππ 
{
∫∫ 
await
ªª  
UserServiceManager
ªª 0
.
ªª0 1
Instance
ªª1 9
.
ªª9 :
Client
ªª: @
.
ªª@ A
LogoutAsync
ªªA L
(
ªªL M
UserSession
ªªM X
.
ªªX Y
SessionToken
ªªY e
)
ªªe f
;
ªªf g
}
ºº 
catch
ΩΩ 
(
ΩΩ 
	Exception
ΩΩ $
ex
ΩΩ% '
)
ΩΩ' (
{
ææ 
System
øø 
.
øø 
Diagnostics
øø *
.
øø* +
Debug
øø+ 0
.
øø0 1
	WriteLine
øø1 :
(
øø: ;
$"
øø; =
$str
øø= R
{
øøR S
ex
øøS U
.
øøU V
Message
øøV ]
}
øø] ^
"
øø^ _
)
øø_ `
;
øø` a
}
¿¿ 
UserSession
¬¬ 
.
¬¬  

EndSession
¬¬  *
(
¬¬* +
)
¬¬+ ,
;
¬¬, -
NavigationHelper
√√ $
.
√√$ %

NavigateTo
√√% /
(
√√/ 0
this
√√0 4
,
√√4 5
new
√√6 9
Login
√√: ?
(
√√? @
)
√√@ A
)
√√A B
;
√√B C
}
ƒƒ 
else
≈≈ 
{
∆∆ 
string
«« 
msg
«« 
=
««  
(
««! "
response
««" *
.
««* +

MessageKey
««+ 5
==
««6 8

ServerKeys
««9 C
.
««C D
UsernameInUse
««D Q
)
««Q R
?
»» 
Lang
»» 
.
»» (
Global_Error_UsernameInUse
»» 9
:
…… 
	GetString
…… #
(
……# $
response
……$ ,
.
……, -

MessageKey
……- 7
)
……7 8
;
……8 9
	ShowError
ÀÀ 
(
ÀÀ 
Lang
ÀÀ "
.
ÀÀ" # 
Global_Title_Error
ÀÀ# 5
,
ÀÀ5 6
msg
ÀÀ7 :
)
ÀÀ: ;
;
ÀÀ; <
}
ÃÃ 
}
ÕÕ 
catch
ŒŒ 
(
ŒŒ 
	Exception
ŒŒ 
ex
ŒŒ 
)
ŒŒ  
{
œœ 
ExceptionManager
––  
.
––  !
Handle
––! '
(
––' (
ex
––( *
,
––* +
this
––, 0
)
––0 1
;
––1 2
}
—— 
finally
““ 
{
”” 
if
‘‘ 
(
‘‘ "
ButtonUpdateUsername
‘‘ (
!=
‘‘) +
null
‘‘, 0
&&
‘‘1 3
!
‘‘4 5"
ButtonUpdateUsername
‘‘5 I
.
‘‘I J
	IsEnabled
‘‘J S
&&
‘‘T V
Application
‘‘W b
.
‘‘b c
Current
‘‘c j
.
‘‘j k

MainWindow
‘‘k u
==
‘‘v x
this
‘‘y }
)
‘‘} ~
{
’’ "
ButtonUpdateUsername
÷÷ (
.
÷÷( )
	IsEnabled
÷÷) 2
=
÷÷3 4
true
÷÷5 9
;
÷÷9 :
}
◊◊ 
}
ÿÿ 
}
ŸŸ 	
private
€€ 
async
€€ 
void
€€ ,
ButtonUpdatePersonalInfo_Click
€€ 9
(
€€9 :
object
€€: @
sender
€€A G
,
€€G H
RoutedEventArgs
€€I X
e
€€Y Z
)
€€Z [
{
‹‹ 	
string
›› 
name
›› 
=
›› 
TextBoxName
›› %
.
››% &
Text
››& *
.
››* +
Trim
››+ /
(
››/ 0
)
››0 1
;
››1 2
string
ﬁﬁ 
lastName
ﬁﬁ 
=
ﬁﬁ 
TextBoxLastName
ﬁﬁ -
.
ﬁﬁ- .
Text
ﬁﬁ. 2
.
ﬁﬁ2 3
Trim
ﬁﬁ3 7
(
ﬁﬁ7 8
)
ﬁﬁ8 9
;
ﬁﬁ9 :
var
‡‡ 
button
‡‡ 
=
‡‡ 
sender
‡‡ 
as
‡‡  "
Button
‡‡# )
;
‡‡) *
if
‚‚ 
(
‚‚ 
button
‚‚ 
!=
‚‚ 
null
‚‚ 
)
‚‚ 
{
„„ 
button
‰‰ 
.
‰‰ 
	IsEnabled
‰‰  
=
‰‰! "
false
‰‰# (
;
‰‰( )
Mouse
ÂÂ 
.
ÂÂ 
OverrideCursor
ÂÂ $
=
ÂÂ% &
Cursors
ÂÂ' .
.
ÂÂ. /
Wait
ÂÂ/ 3
;
ÂÂ3 4
}
ÊÊ 
try
ËË 
{
ÈÈ 
var
ÍÍ 
response
ÍÍ 
=
ÍÍ 
await
ÍÍ $ 
UserServiceManager
ÍÍ% 7
.
ÍÍ7 8
Instance
ÍÍ8 @
.
ÍÍ@ A
Client
ÍÍA G
.
ÍÍG H%
UpdatePersonalInfoAsync
ÍÍH _
(
ÍÍ_ `
UserSession
ÍÍ` k
.
ÍÍk l
SessionToken
ÍÍl x
,
ÍÍx y
name
ÍÍz ~
,
ÍÍ~ 
lastNameÍÍÄ à
)ÍÍà â
;ÍÍâ ä
if
ÎÎ 
(
ÎÎ 
response
ÎÎ 
.
ÎÎ 
Success
ÎÎ $
)
ÎÎ$ %
{
ÏÏ 
UserSession
ÌÌ 
.
ÌÌ  
Name
ÌÌ  $
=
ÌÌ% &
name
ÌÌ' +
;
ÌÌ+ ,
UserSession
ÓÓ 
.
ÓÓ  
LastName
ÓÓ  (
=
ÓÓ) *
lastName
ÓÓ+ 3
;
ÓÓ3 4
new
ÔÔ 
CustomMessageBox
ÔÔ (
(
ÔÔ( )
Lang
ÔÔ) -
.
ÔÔ- ."
Global_Title_Success
ÔÔ. B
,
ÔÔB C
Lang
ÔÔD H
.
ÔÔH I0
"EditProfile_Label_SuccesUpdateInfo
ÔÔI k
,
ÔÔk l
this
 
,
 
MessageBoxType
 ,
.
, -
Success
- 4
)
4 5
.
5 6

ShowDialog
6 @
(
@ A
)
A B
;
B C
}
ÒÒ 
else
ÚÚ 
{
ÛÛ 
	ShowError
ÙÙ 
(
ÙÙ 
Lang
ÙÙ "
.
ÙÙ" # 
Global_Title_Error
ÙÙ# 5
,
ÙÙ5 6
	GetString
ÙÙ7 @
(
ÙÙ@ A
response
ÙÙA I
.
ÙÙI J

MessageKey
ÙÙJ T
)
ÙÙT U
)
ÙÙU V
;
ÙÙV W
}
ıı 
}
ˆˆ 
catch
˜˜ 
(
˜˜ 
	Exception
˜˜ 
ex
˜˜ 
)
˜˜  
{
¯¯ 
ExceptionManager
˘˘  
.
˘˘  !
Handle
˘˘! '
(
˘˘' (
ex
˘˘( *
,
˘˘* +
this
˘˘, 0
)
˘˘0 1
;
˘˘1 2
}
˙˙ 
finally
˚˚ 
{
¸¸ 
if
˝˝ 
(
˝˝ 
button
˝˝ 
!=
˝˝ 
null
˝˝ "
)
˝˝" #
{
˛˛ 
button
ˇˇ 
.
ˇˇ 
	IsEnabled
ˇˇ $
=
ˇˇ% &
true
ˇˇ' +
;
ˇˇ+ ,
Mouse
ÄÄ 
.
ÄÄ 
OverrideCursor
ÄÄ (
=
ÄÄ) *
null
ÄÄ+ /
;
ÄÄ/ 0
}
ÅÅ 
}
ÇÇ 
}
ÉÉ 	
private
ÖÖ 
async
ÖÖ 
void
ÖÖ &
ButtonRemoveSocial_Click
ÖÖ 3
(
ÖÖ3 4
object
ÖÖ4 :
sender
ÖÖ; A
,
ÖÖA B
RoutedEventArgs
ÖÖC R
e
ÖÖS T
)
ÖÖT U
{
ÜÜ 	
if
áá 
(
áá 
!
áá 
(
áá 
sender
áá 
is
áá 
Button
áá "
button
áá# )
)
áá) *
||
áá+ -
!
áá. /
(
áá/ 0
button
áá0 6
.
áá6 7
Tag
áá7 :
is
áá; =
int
áá> A
socialId
ááB J
)
ááJ K
)
ááK L
{
àà 
return
ââ 
;
ââ 
}
ää 
if
åå 
(
åå 
socialId
åå 
<=
åå 
$num
åå 
)
åå 
{
çç 
RemoveFromUiList
éé  
(
éé  !
socialId
éé! )
)
éé) *
;
éé* +
return
èè 
;
èè 
}
êê 
await
íí ,
RemoveRemoteSocialNetworkAsync
íí 0
(
íí0 1
socialId
íí1 9
)
íí9 :
;
íí: ;
}
ìì 	
private
ïï 
async
ïï 
Task
ïï ,
RemoveRemoteSocialNetworkAsync
ïï 9
(
ïï9 :
int
ïï: =
socialId
ïï> F
)
ïïF G
{
ññ 	
try
óó 
{
òò 
var
ôô 
response
ôô 
=
ôô 
await
ôô $ 
UserServiceManager
ôô% 7
.
ôô7 8
Instance
ôô8 @
.
ôô@ A
Client
ôôA G
.
ôôG H&
RemoveSocialNetworkAsync
ôôH `
(
ôô` a
UserSession
ôôa l
.
ôôl m
SessionToken
ôôm y
,
ôôy z
socialIdôô{ É
)ôôÉ Ñ
;ôôÑ Ö
if
õõ 
(
õõ 
response
õõ 
.
õõ 
Success
õõ $
)
õõ$ %
{
úú 
RemoveFromUiList
ùù $
(
ùù$ %
socialId
ùù% -
)
ùù- .
;
ùù. /
RemoveFromSession
ûû %
(
ûû% &
socialId
ûû& .
)
ûû. /
;
ûû/ 0
}
üü 
else
†† 
{
°° 
string
¢¢ 
errorMessage
¢¢ '
=
¢¢( )
	GetString
¢¢* 3
(
¢¢3 4
response
¢¢4 <
.
¢¢< =

MessageKey
¢¢= G
)
¢¢G H
;
¢¢H I
	ShowError
££ 
(
££ 
Lang
££ "
.
££" # 
Global_Title_Error
££# 5
,
££5 6
errorMessage
££7 C
)
££C D
;
££D E
}
§§ 
}
•• 
catch
¶¶ 
(
¶¶ 
	Exception
¶¶ 
ex
¶¶ 
)
¶¶  
{
ßß 
ExceptionManager
®®  
.
®®  !
Handle
®®! '
(
®®' (
ex
®®( *
,
®®* +
this
®®, 0
)
®®0 1
;
®®1 2
}
©© 
}
™™ 	
private
¨¨ 
void
¨¨ 
RemoveFromUiList
¨¨ %
(
¨¨% &
int
¨¨& )
socialId
¨¨* 2
)
¨¨2 3
{
≠≠ 	
var
ÆÆ 
item
ÆÆ 
=
ÆÆ  
SocialNetworksList
ÆÆ )
.
ÆÆ) *
FirstOrDefault
ÆÆ* 8
(
ÆÆ8 9
s
ÆÆ9 :
=>
ÆÆ; =
s
ÆÆ> ?
.
ÆÆ? @
SocialNetworkId
ÆÆ@ O
==
ÆÆP R
socialId
ÆÆS [
)
ÆÆ[ \
;
ÆÆ\ ]
if
ØØ 
(
ØØ 
item
ØØ 
!=
ØØ 
null
ØØ 
)
ØØ 
{
∞∞  
SocialNetworksList
±± "
.
±±" #
Remove
±±# )
(
±±) *
item
±±* .
)
±±. /
;
±±/ 0
}
≤≤ 
}
≥≥ 	
private
µµ 
static
µµ 
void
µµ 
RemoveFromSession
µµ -
(
µµ- .
int
µµ. 1
socialId
µµ2 :
)
µµ: ;
{
∂∂ 	
var
∑∑ 
sessionItem
∑∑ 
=
∑∑ 
UserSession
∑∑ )
.
∑∑) *
SocialNetworks
∑∑* 8
.
∑∑8 9
FirstOrDefault
∑∑9 G
(
∑∑G H
s
∑∑H I
=>
∑∑J L
s
∑∑M N
.
∑∑N O
SocialNetworkId
∑∑O ^
==
∑∑_ a
socialId
∑∑b j
)
∑∑j k
;
∑∑k l
if
∏∏ 
(
∏∏ 
sessionItem
∏∏ 
!=
∏∏ 
null
∏∏ #
)
∏∏# $
{
ππ 
UserSession
∫∫ 
.
∫∫ 
SocialNetworks
∫∫ *
.
∫∫* +
Remove
∫∫+ 1
(
∫∫1 2
sessionItem
∫∫2 =
)
∫∫= >
;
∫∫> ?
}
ªª 
}
ºº 	
private
ææ 
async
ææ 
void
ææ #
ButtonAddSocial_Click
ææ 0
(
ææ0 1
object
ææ1 7
sender
ææ8 >
,
ææ> ?
RoutedEventArgs
ææ@ O
e
ææP Q
)
ææQ R
{
øø 	
string
¿¿ 
account
¿¿ 
=
¿¿ 
TextBoxNewSocial
¿¿ -
.
¿¿- .
Text
¿¿. 2
.
¿¿2 3
Trim
¿¿3 7
(
¿¿7 8
)
¿¿8 9
;
¿¿9 :
if
¡¡ 
(
¡¡ 
string
¡¡ 
.
¡¡ 
IsNullOrEmpty
¡¡ $
(
¡¡$ %
account
¡¡% ,
)
¡¡, -
)
¡¡- .
{
¬¬ 
return
√√ 
;
√√ 
}
ƒƒ 
var
∆∆ 
button
∆∆ 
=
∆∆ 
sender
∆∆ 
as
∆∆  "
Button
∆∆# )
;
∆∆) *
if
«« 
(
«« 
button
«« 
!=
«« 
null
«« 
)
«« 
button
««  &
.
««& '
	IsEnabled
««' 0
=
««1 2
false
««3 8
;
««8 9
try
…… 
{
   
var
ÀÀ 
response
ÀÀ 
=
ÀÀ 
await
ÀÀ $ 
UserServiceManager
ÀÀ% 7
.
ÀÀ7 8
Instance
ÀÀ8 @
.
ÀÀ@ A
Client
ÀÀA G
.
ÀÀG H#
AddSocialNetworkAsync
ÀÀH ]
(
ÀÀ] ^
UserSession
ÀÀ^ i
.
ÀÀi j
SessionToken
ÀÀj v
,
ÀÀv w
account
ÀÀx 
)ÀÀ Ä
;ÀÀÄ Å
if
ÕÕ 
(
ÕÕ 
response
ÕÕ 
.
ÕÕ 
Success
ÕÕ $
)
ÕÕ$ %
{
ŒŒ 
TextBoxNewSocial
œœ $
.
œœ$ %
Text
œœ% )
=
œœ* +
string
œœ, 2
.
œœ2 3
Empty
œœ3 8
;
œœ8 9
var
—— 
	newSocial
—— !
=
——" #
new
——$ '
SocialNetworkDTO
——( 8
{
““ 
Account
”” 
=
””  !
account
””" )
,
””) *
SocialNetworkId
‘‘ '
=
‘‘( )
response
‘‘* 2
.
‘‘2 3 
NewSocialNetworkId
‘‘3 E
}
’’ 
;
’’  
SocialNetworksList
◊◊ &
.
◊◊& '
Add
◊◊' *
(
◊◊* +
	newSocial
◊◊+ 4
)
◊◊4 5
;
◊◊5 6
UserSession
ŸŸ 
.
ŸŸ  
SocialNetworks
ŸŸ  .
.
ŸŸ. /
Add
ŸŸ/ 2
(
ŸŸ2 3
	newSocial
ŸŸ3 <
)
ŸŸ< =
;
ŸŸ= >
new
€€ 
CustomMessageBox
€€ (
(
€€( )
Lang
€€) -
.
€€- ."
Global_Title_Success
€€. B
,
€€B C
$str
€€D H
,
€€I J
this
€€J N
,
€€N O
MessageBoxType
€€P ^
.
€€^ _
Success
€€_ f
)
€€f g
.
€€g h

ShowDialog
€€h r
(
€€r s
)
€€s t
;
€€t u
}
‹‹ 
else
›› 
{
ﬁﬁ 
	ShowError
ﬂﬂ 
(
ﬂﬂ 
Lang
ﬂﬂ "
.
ﬂﬂ" # 
Global_Title_Error
ﬂﬂ# 5
,
ﬂﬂ5 6
	GetString
ﬂﬂ7 @
(
ﬂﬂ@ A
response
ﬂﬂA I
.
ﬂﬂI J

MessageKey
ﬂﬂJ T
)
ﬂﬂT U
)
ﬂﬂU V
;
ﬂﬂV W
}
‡‡ 
}
·· 
catch
‚‚ 
(
‚‚ 
	Exception
‚‚ 
ex
‚‚ 
)
‚‚  
{
„„ 
ExceptionManager
‰‰  
.
‰‰  !
Handle
‰‰! '
(
‰‰' (
ex
‰‰( *
,
‰‰* +
this
‰‰, 0
)
‰‰0 1
;
‰‰1 2
}
ÂÂ 
finally
ÊÊ 
{
ÁÁ 
if
ËË 
(
ËË 
button
ËË 
!=
ËË 
null
ËË "
)
ËË" #
button
ËË$ *
.
ËË* +
	IsEnabled
ËË+ 4
=
ËË5 6
true
ËË7 ;
;
ËË; <
}
ÈÈ 
}
ÍÍ 	
private
ÏÏ 
void
ÏÏ 
ButtonBack_Click
ÏÏ %
(
ÏÏ% &
object
ÏÏ& ,
sender
ÏÏ- 3
,
ÏÏ3 4
RoutedEventArgs
ÏÏ5 D
e
ÏÏE F
)
ÏÏF G
{
ÌÌ 	
NavigationHelper
ÓÓ 
.
ÓÓ 

NavigateTo
ÓÓ '
(
ÓÓ' (
this
ÓÓ( ,
,
ÓÓ, -
this
ÓÓ. 2
.
ÓÓ2 3
Owner
ÓÓ3 8
??
ÓÓ9 ;
new
ÓÓ< ?
PlayerProfile
ÓÓ@ M
(
ÓÓM N
)
ÓÓN O
)
ÓÓO P
;
ÓÓP Q
}
ÔÔ 	
private
ÒÒ 
void
ÒÒ 
	ShowError
ÒÒ 
(
ÒÒ 
string
ÒÒ %
title
ÒÒ& +
,
ÒÒ+ ,
string
ÒÒ- 3
message
ÒÒ4 ;
)
ÒÒ; <
{
ÚÚ 	
var
ÛÛ 
msgBox
ÛÛ 
=
ÛÛ 
new
ÛÛ 
CustomMessageBox
ÛÛ -
(
ÛÛ- .
title
ÛÛ. 3
,
ÛÛ3 4
message
ÛÛ5 <
,
ÛÛ< =
this
ÛÛ> B
,
ÛÛB C
MessageBoxType
ÛÛD R
.
ÛÛR S
Error
ÛÛS X
)
ÛÛX Y
;
ÛÛY Z
msgBox
ÙÙ 
.
ÙÙ 

ShowDialog
ÙÙ 
(
ÙÙ 
)
ÙÙ 
;
ÙÙ  
}
ıı 	
private
˜˜ 
void
˜˜ 
ShowSuccess
˜˜  
(
˜˜  !
string
˜˜! '
title
˜˜( -
,
˜˜- .
string
˜˜/ 5
message
˜˜6 =
)
˜˜= >
{
¯¯ 	
var
˘˘ 
msgBox
˘˘ 
=
˘˘ 
new
˘˘ 
CustomMessageBox
˘˘ -
(
˘˘- .
title
˘˘. 3
,
˘˘3 4
message
˘˘5 <
,
˘˘< =
this
˘˘> B
,
˘˘B C
MessageBoxType
˘˘D R
.
˘˘R S
Success
˘˘S Z
)
˘˘Z [
;
˘˘[ \
msgBox
˙˙ 
.
˙˙ 

ShowDialog
˙˙ 
(
˙˙ 
)
˙˙ 
;
˙˙  
}
˚˚ 	
}
¸¸ 
}˝˝ »
?C:\MemoryGame\Client\Client\Views\Multiplayer\JoinLobby.xaml.cs
	namespace 	
Client
 
. 
Views 
. 
Multiplayer "
{		 
public 

partial 
class 
	JoinLobby "
:# $
Window% +
{ 
private 
const 
int 
GAME_CODE_LENGTH *
=+ ,
$num- .
;. /
public 
	JoinLobby 
( 
) 
{ 	
InitializeComponent 
(  
)  !
;! "
} 	
private 
void (
NumericOnly_PreviewTextInput 1
(1 2
object2 8
sender9 ?
,? @$
TextCompositionEventArgsA Y
eZ [
)[ \
{ 	
e 
. 
Handled 
= 
new 
Regex !
(! "
$str" +
)+ ,
., -
IsMatch- 4
(4 5
e5 6
.6 7
Text7 ;
); <
;< =
} 	
private 
void "
TextBox_PreviewKeyDown +
(+ ,
object, 2
sender3 9
,9 :
KeyEventArgs; G
eH I
)I J
{ 	
if 
( 
e 
. 
Key 
== 
Key 
. 
Space "
)" #
{ 
e 
. 
Handled 
= 
true  
;  !
} 
}   	
private"" 
void"" "
ButtonAcceptCode_Click"" +
(""+ ,
object"", 2
sender""3 9
,""9 :
RoutedEventArgs""; J
e""K L
)""L M
{## 	
string$$ 
	lobbyCode$$ 
=$$ 
TextBoxLobbyCode$$ /
.$$/ 0
Text$$0 4
?$$4 5
.$$5 6
Trim$$6 :
($$: ;
)$$; <
;$$< =
LabelCodeError%% 
.%% 
Content%% "
=%%# $
$str%%% '
;%%' (
ValidationCode'' 
validationCode'' )
=''* +
ValidateVerifyCode'', >
(''> ?
	lobbyCode''? H
,''H I
GAME_CODE_LENGTH''J Z
)''Z [
;''[ \
if)) 
()) 
validationCode)) 
!=)) !
ValidationCode))" 0
.))0 1
Success))1 8
)))8 9
{** 
LabelCodeError++ 
.++ 
Content++ &
=++' (
	GetString++) 2
(++2 3
validationCode++3 A
)++A B
;++B C
return,, 
;,, 
}-- 
var// 
lobbyWindow// 
=// 
new// !
Lobby//" '
.//' (
Lobby//( -
(//- .
	lobbyCode//. 7
)//7 8
;//8 9
NavigationHelper00 
.00 

NavigateTo00 '
(00' (
this00( ,
,00, -
lobbyWindow00. 9
)009 :
;00: ;
}11 	
private33 
void33 &
ButtonBackToMainMenu_Click33 /
(33/ 0
object330 6
sender337 =
,33= >
RoutedEventArgs33? N
e33O P
)33P Q
{44 	
NavigationHelper55 
.55 

NavigateTo55 '
(55' (
this55( ,
,55, -
this55. 2
.552 3
Owner553 8
??559 ;
new55< ?
MultiplayerMenu55@ O
(55O P
)55P Q
)55Q R
;55R S
}66 	
}77 
}88 ì
EC:\MemoryGame\Client\Client\Views\Multiplayer\MultiplayerMenu.xaml.cs
	namespace		 	
Client		
 
.		 
Views		 
.		 
Multiplayer		 "
{

 
public 

partial 
class 
MultiplayerMenu (
:) *
Window+ 1
{ 
public 
MultiplayerMenu 
( 
)  
{ 	
InitializeComponent 
(  
)  !
;! "
if 
( 
UserSession 
. 
IsGuest #
)# $
{ 
ButtonCreateLobby !
.! "
	IsEnabled" +
=, -
false. 3
;3 4
ButtonCreateLobby !
.! "
Opacity" )
=* +
$num, /
;/ 0
} 
} 	
private 
void #
ButtonCreateLobby_Click ,
(, -
object- 3
sender4 :
,: ;
RoutedEventArgs< K
eL M
)M N
{ 	
if 
( 
UserSession 
. 
IsGuest #
)# $
{ 
new 
CustomMessageBox $
($ %
Lang   
.   -
!Global_Title_NotAvailableFunction   :
,  : ;
Lang!! 
.!! )
Global_Error_GuestsNotAllowed!! 6
,!!6 7
this"" 
,"" 
MessageBoxType"" (
.""( )
Warning"") 0
)""0 1
.""1 2

ShowDialog""2 <
(""< =
)""= >
;""> ?
return## 
;## 
}$$ 
NavigationHelper&& 
.&& 

NavigateTo&& '
(&&' (
this&&( ,
,&&, -
new&&. 1
	HostLobby&&2 ;
(&&; <
)&&< =
)&&= >
;&&> ?
}'' 	
private)) 
void)) !
ButtonJoinLobby_Click)) *
())* +
object))+ 1
sender))2 8
,))8 9
RoutedEventArgs)): I
e))J K
)))K L
{** 	
NavigationHelper++ 
.++ 

NavigateTo++ '
(++' (
this++( ,
,++, -
new++. 1
	JoinLobby++2 ;
(++; <
)++< =
)++= >
;++> ?
},, 	
private.. 
void.. &
ButtonBackToMainMenu_Click.. /
(../ 0
object..0 6
sender..7 =
,..= >
RoutedEventArgs..? N
e..O P
)..P Q
{// 	
NavigationHelper00 
.00 

NavigateTo00 '
(00' (
this00( ,
,00, -
this00. 2
.002 3
Owner003 8
??009 ;
new00< ?
MainMenu00@ H
(00H I
)00I J
)00J K
;00K L
}11 	
}22 
}33 ∏q
2C:\MemoryGame\Client\Client\Views\MainMenu.xaml.cs
	namespace 	
Client
 
. 
Views 
{ 
public 

partial 
class 
MainMenu !
:" #
Window$ *
{ 
private 
DispatcherTimer 
_keepAliveTimer  /
;/ 0
private 
const 
int '
KEEP_ALIVE_INTERVAL_SECONDS 5
=6 7
$num8 ;
;; <
public 
MainMenu 
( 
) 
{ 	
InitializeComponent 
(  
)  !
;! "
UsernameDisplay 
. 
Content #
=$ %
UserSession& 1
.1 2
Username2 :
;: ;
if   
(   
UserSession   
.   
IsGuest   #
)  # $
{!! 
ButtonSignIn"" 
."" 

Visibility"" '
=""( )

Visibility""* 4
.""4 5
Visible""5 <
;""< =
ProfilePicture## 
.## 
Source## %
=##& '
null##( ,
;##, -
}$$ 
else%% 
{&& 
ButtonSignIn'' 
.'' 

Visibility'' '
=''( )

Visibility''* 4
.''4 5
	Collapsed''5 >
;''> ?
UserSession(( 
.(( 
ProfileUpdated(( *
+=((+ -
OnProfileUpdated((. >
;((> ?
_)) 
=)) 
LoadAvatarAsync)) #
())# $
)))$ %
;))% &
InitializeKeepAlive** #
(**# $
)**$ %
;**% &
}++ 
},, 	
private00 
void00 $
ButtonSinglePlayer_Click00 -
(00- .
object00. 4
sender005 ;
,00; <
RoutedEventArgs00= L
e00M N
)00N O
{11 	
NavigationHelper22 
.22 

NavigateTo22 '
(22' (
this22( ,
,22, -
new22. 1
SelectDifficulty222 B
(22B C
)22C D
)22D E
;22E F
}33 	
private55 
void55 #
ButtonMultiplayer_Click55 ,
(55, -
object55- 3
sender554 :
,55: ;
RoutedEventArgs55< K
e55L M
)55M N
{66 	
NavigationHelper77 
.77 

NavigateTo77 '
(77' (
this77( ,
,77, -
new77. 1
MultiplayerMenu772 A
(77A B
)77B C
)77C D
;77D E
}88 	
private:: 
void::  
ButtonSettings_Click:: )
(::) *
object::* 0
sender::1 7
,::7 8
RoutedEventArgs::9 H
e::I J
)::J K
{;; 	
NavigationHelper<< 
.<< 

NavigateTo<< '
(<<' (
this<<( ,
,<<, -
new<<. 1
Settings<<2 :
(<<: ;
)<<; <
)<<< =
;<<= >
}== 	
private?? 
void?? 
ButtonProfile_Click?? (
(??( )
object??) /
sender??0 6
,??6 7
RoutedEventArgs??8 G
e??H I
)??I J
{@@ 	
ifAA 
(AA 
UserSessionAA 
.AA 
IsGuestAA #
)AA# $
{BB 
stringCC 
messageCC 
=CC  
stringCC! '
.CC' (
FormatCC( .
(CC. /
LangCC/ 3
.CC3 4,
 PlayerProfile_Message_NotAvaibleCC4 T
,CCT U
UserSessionCCV a
.CCa b
UsernameCCb j
)CCj k
;CCk l
varDD 
msgBoxDD 
=DD 
newDD  
CustomMessageBoxDD! 1
(DD1 2
LangEE 
.EE -
!Global_Title_NotAvailableFunctionEE :
,EE: ;
messageEE< C
,EEC D
thisFF 
,FF 
MessageBoxTypeFF (
.FF( )
WarningFF) 0
)FF0 1
;FF1 2
msgBoxGG 
.GG 

ShowDialogGG !
(GG! "
)GG" #
;GG# $
returnHH 
;HH 
}II 
NavigationHelperKK 
.KK 

NavigateToKK '
(KK' (
thisKK( ,
,KK, -
newKK. 1
ProfileKK2 9
.KK9 :
PlayerProfileKK: G
(KKG H
)KKH I
)KKI J
;KKJ K
}LL 	
privateNN 
voidNN 
ButtonSignIn_ClickNN '
(NN' (
objectNN( .
senderNN/ 5
,NN5 6
RoutedEventArgsNN7 F
eNNG H
)NNH I
{OO 	
NavigationHelperPP 
.PP 

NavigateToPP '
(PP' (
thisPP( ,
,PP, -
newPP. 1
RegisterAccountPP2 A
(PPA B
isGuestRegisterPPB Q
:PPQ R
truePPS W
)PPW X
)PPX Y
;PPY Z
}QQ 	
privateSS 
voidSS  
ButtonExitGame_ClickSS )
(SS) *
objectSS* 0
senderSS1 7
,SS7 8
RoutedEventArgsSS9 H
eSSI J
)SSJ K
{TT 	
varUU 
confirmationBoxUU 
=UU  !
newUU" %"
ConfirmationMessageBoxUU& <
(UU< =
LangVV 
.VV !
Global_Title_ExitGameVV *
,VV* +
LangVV, 0
.VV0 1#
Global_Message_ExitGameVV1 H
,VVH I
thisWW 
,WW "
ConfirmationMessageBoxWW ,
.WW, -
ConfirmationBoxTypeWW- @
.WW@ A
CriticWWA G
)WWG H
;WWH I
ifYY 
(YY 
confirmationBoxYY 
.YY  

ShowDialogYY  *
(YY* +
)YY+ ,
==YY- /
trueYY0 4
)YY4 5
{ZZ 
PerformLogout[[ 
([[ 
)[[ 
;[[  
NavigationHelper\\  
.\\  !
ExitApplication\\! 0
(\\0 1
)\\1 2
;\\2 3
}]] 
}^^ 	
privatedd 
asyncdd 
Taskdd 
LoadAvatarAsyncdd *
(dd* +
)dd+ ,
{ee 	
tryff 
{gg 
bytehh 
[hh 
]hh 
avatarByteshh "
=hh# $
awaithh% *
UserServiceManagerhh+ =
.hh= >
Instancehh> F
.hhF G
ClienthhG M
.hhM N
GetUserAvatarAsynchhN `
(hh` a
UserSessionhha l
.hhl m
Emailhhm r
)hhr s
;hhs t
ifjj 
(jj 
avatarBytesjj 
!=jj  "
nulljj# '
&&jj( *
avatarBytesjj+ 6
.jj6 7
Lengthjj7 =
>jj> ?
$numjj@ A
)jjA B
{kk 
ProfilePicturell "
.ll" #
Sourcell# )
=ll* +
ImageHelperll, 7
.ll7 8"
ByteArrayToImageSourcell8 N
(llN O
avatarBytesllO Z
)llZ [
;ll[ \
}mm 
}nn 
catchoo 
(oo 
	Exceptionoo 
exoo 
)oo  
{pp 
Debugqq 
.qq 
	WriteLineqq 
(qq  
$"qq  "
$strqq" D
{qqD E
exqqE G
.qqG H
MessageqqH O
}qqO P
"qqP Q
)qqQ R
;qqR S
}rr 
}ss 	
privateuu 
voiduu 
OnProfileUpdateduu %
(uu% &
)uu& '
{vv 	
UsernameDisplayww 
.ww 
Contentww #
=ww$ %
UserSessionww& 1
.ww1 2
Usernameww2 :
;ww: ;
_xx 
=xx 
LoadAvatarAsyncxx 
(xx  
)xx  !
;xx! "
}yy 	
private{{ 
static{{ 
void{{ 
PerformLogout{{ )
({{) *
){{* +
{|| 	
if}} 
(}} 
UserSession}} 
.}} 
IsGuest}} #
)}}# $
{~~ 
try 
{
ÄÄ  
UserServiceManager
ÅÅ &
.
ÅÅ& '
Instance
ÅÅ' /
.
ÅÅ/ 0
Client
ÅÅ0 6
.
ÅÅ6 7
LogoutGuestAsync
ÅÅ7 G
(
ÅÅG H
UserSession
ÅÅH S
.
ÅÅS T
SessionToken
ÅÅT `
)
ÅÅ` a
;
ÅÅa b
}
ÇÇ 
catch
ÉÉ 
(
ÉÉ 
	Exception
ÉÉ  
ex
ÉÉ! #
)
ÉÉ# $
{
ÑÑ 
Debug
ÖÖ 
.
ÖÖ 
	WriteLine
ÖÖ #
(
ÖÖ# $
$"
ÖÖ$ &
$str
ÖÖ& G
{
ÖÖG H
ex
ÖÖH J
.
ÖÖJ K
Message
ÖÖK R
}
ÖÖR S
"
ÖÖS T
)
ÖÖT U
;
ÖÖU V
}
ÜÜ 
}
áá 
else
àà 
{
ââ 
try
ää 
{
ãã 
if
åå 
(
åå  
UserServiceManager
åå *
.
åå* +
Instance
åå+ 3
.
åå3 4
Client
åå4 :
.
åå: ;
State
åå; @
==
ååA C 
CommunicationState
ååD V
.
ååV W
Opened
ååW ]
)
åå] ^
{
çç  
UserServiceManager
éé *
.
éé* +
Instance
éé+ 3
.
éé3 4
Client
éé4 :
.
éé: ;
LogoutAsync
éé; F
(
ééF G
UserSession
ééG R
.
ééR S
SessionToken
ééS _
)
éé_ `
;
éé` a
}
èè 
}
êê 
catch
ëë 
(
ëë 
	Exception
ëë  
ex
ëë! #
)
ëë# $
{
íí 
Debug
ìì 
.
ìì 
	WriteLine
ìì #
(
ìì# $
$"
ìì$ &
$str
ìì& H
{
ììH I
ex
ììI K
.
ììK L
Message
ììL S
}
ììS T
"
ììT U
)
ììU V
;
ììV W
}
îî 
}
ïï 
}
ññ 	
private
úú 
void
úú !
InitializeKeepAlive
úú (
(
úú( )
)
úú) *
{
ùù 	
_keepAliveTimer
ûû 
=
ûû 
new
ûû !
DispatcherTimer
ûû" 1
(
ûû1 2
)
ûû2 3
;
ûû3 4
_keepAliveTimer
üü 
.
üü 
Interval
üü $
=
üü% &
TimeSpan
üü' /
.
üü/ 0
FromSeconds
üü0 ;
(
üü; <)
KEEP_ALIVE_INTERVAL_SECONDS
üü< W
)
üüW X
;
üüX Y
_keepAliveTimer
†† 
.
†† 
Tick
††  
+=
††! #
async
††$ )
(
††* +
s
††+ ,
,
††, -
e
††. /
)
††/ 0
=>
††1 3
await
††4 9 
SendHeartbeatAsync
††: L
(
††L M
)
††M N
;
††N O
_keepAliveTimer
°° 
.
°° 
Start
°° !
(
°°! "
)
°°" #
;
°°# $
}
¢¢ 	
private
§§ 
async
§§ 
Task
§§  
SendHeartbeatAsync
§§ -
(
§§- .
)
§§. /
{
•• 	
try
¶¶ 
{
ßß 
var
®® 
response
®® 
=
®® 
await
®® $ 
UserServiceManager
®®% 7
.
®®7 8
Instance
®®8 @
.
®®@ A
Client
®®A G
.
®®G H
RenewSessionAsync
®®H Y
(
®®Y Z
UserSession
®®Z e
.
®®e f
SessionToken
®®f r
)
®®r s
;
®®s t
if
™™ 
(
™™ 
!
™™ 
response
™™ 
.
™™ 
Success
™™ %
)
™™% &
{
´´ 
Debug
¨¨ 
.
¨¨ 
	WriteLine
¨¨ #
(
¨¨# $
$str
¨¨$ X
)
¨¨X Y
;
¨¨Y Z%
HandleSessionExpiration
≠≠ +
(
≠≠+ ,
)
≠≠, -
;
≠≠- .
}
ÆÆ 
}
ØØ 
catch
∞∞ 
(
∞∞ 
	Exception
∞∞ 
ex
∞∞ 
)
∞∞  
{
±± 
Debug
≤≤ 
.
≤≤ 
	WriteLine
≤≤ 
(
≤≤  
$"
≤≤  "
$str
≤≤" ?
{
≤≤? @
ex
≤≤@ B
.
≤≤B C
Message
≤≤C J
}
≤≤J K
"
≤≤K L
)
≤≤L M
;
≤≤M N
}
≥≥ 
}
¥¥ 	
private
∂∂ 
void
∂∂ %
HandleSessionExpiration
∂∂ ,
(
∂∂, -
)
∂∂- .
{
∑∑ 	
_keepAliveTimer
∏∏ 
?
∏∏ 
.
∏∏ 
Stop
∏∏ !
(
∏∏! "
)
∏∏" #
;
∏∏# $
new
∫∫ 
CustomMessageBox
∫∫  
(
∫∫  !
Lang
ªª 
.
ªª  
Global_Title_Error
ªª '
,
ªª' (
Lang
ºº 
.
ºº )
Global_Error_SessionExpired
ºº 0
,
ºº0 1
this
ΩΩ 
,
ΩΩ 
MessageBoxType
ææ 
.
ææ 
Error
ææ $
)
ææ$ %
.
ææ% &

ShowDialog
ææ& 0
(
ææ0 1
)
ææ1 2
;
ææ2 3
UserSession
¿¿ 
.
¿¿ 

EndSession
¿¿ "
(
¿¿" #
)
¿¿# $
;
¿¿$ %
NavigationHelper
¬¬ 
.
¬¬ 

NavigateTo
¬¬ '
(
¬¬' (
this
¬¬( ,
,
¬¬, -
new
¬¬. 1
Login
¬¬2 7
(
¬¬7 8
)
¬¬8 9
)
¬¬9 :
;
¬¬: ;
}
√√ 	
	protected
…… 
override
…… 
void
…… 
OnClosed
……  (
(
……( )
	EventArgs
……) 2
e
……3 4
)
……4 5
{
   	
_keepAliveTimer
ÀÀ 
?
ÀÀ 
.
ÀÀ 
Stop
ÀÀ !
(
ÀÀ! "
)
ÀÀ" #
;
ÀÀ# $
UserSession
ÃÃ 
.
ÃÃ 
ProfileUpdated
ÃÃ &
-=
ÃÃ' )
OnProfileUpdated
ÃÃ* :
;
ÃÃ: ;
if
ŒŒ 
(
ŒŒ 
Application
ŒŒ 
.
ŒŒ 
Current
ŒŒ #
.
ŒŒ# $

MainWindow
ŒŒ$ .
==
ŒŒ/ 1
this
ŒŒ2 6
)
ŒŒ6 7
{
œœ 
PerformLogout
–– 
(
–– 
)
–– 
;
––  
}
—— 
base
““ 
.
““ 
OnClosed
““ 
(
““ 
e
““ 
)
““ 
;
““ 
}
”” 	
}
÷÷ 
}◊◊ ì¢
5C:\MemoryGame\Client\Client\Views\Lobby\Lobby.xaml.cs
	namespace 	
Client
 
. 
Views 
. 
Lobby 
{ 
public 

partial 
class 
Lobby 
:  
Window! '
{ 
private 
readonly 
string 

_lobbyCode  *
;* +
private 
bool 
_isConnected !
=" #
false$ )
;) *
private 
bool 
_isGameStarting $
=% &
false' ,
;, -
private 
List 
< 
LobbyPlayerInfo $
>$ %
_currentPlayers& 5
=6 7
new8 ;
List< @
<@ A
LobbyPlayerInfoA P
>P Q
(Q R
)R S
;S T
public 
Lobby 
( 
string 
	lobbyCode %
)% &
{ 	
InitializeComponent   
(    
)    !
;  ! "

_lobbyCode!! 
=!! 
	lobbyCode!! "
;!!" #
if## 
(## 
LabelLobbyCode## 
!=## !
null##" &
)##& '
{$$ 
LabelLobbyCode%% 
.%% 
Content%% &
=%%' (

_lobbyCode%%) 3
;%%3 4
}&& 
ConfigureEvents(( 
((( 
)(( 
;(( 
})) 	
private-- 
void-- 
ConfigureEvents-- $
(--$ %
)--% &
{.. 	
GameServiceManager// 
.// 
Instance// '
.//' (
PlayerListUpdated//( 9
+=//: <
OnPlayerListUpdated//= P
;//P Q
GameServiceManager00 
.00 
Instance00 '
.00' (
GameStarted00( 3
+=004 6
OnGameStarted007 D
;00D E
GameServiceManager11 
.11 
Instance11 '
.11' (
ChatMessageReceived11( ;
+=11< >!
OnChatMessageReceived11? T
;11T U
GameServiceManager22 
.22 
Instance22 '
.22' (

PlayerLeft22( 2
+=223 5
OnPlayerLeft226 B
;22B C
}33 	
private55 
void55 
UnsubscribeEvents55 &
(55& '
)55' (
{66 	
GameServiceManager77 
.77 
Instance77 '
.77' (
PlayerListUpdated77( 9
-=77: <
OnPlayerListUpdated77= P
;77P Q
GameServiceManager88 
.88 
Instance88 '
.88' (
GameStarted88( 3
-=884 6
OnGameStarted887 D
;88D E
GameServiceManager99 
.99 
Instance99 '
.99' (
ChatMessageReceived99( ;
-=99< >!
OnChatMessageReceived99? T
;99T U
GameServiceManager:: 
.:: 
Instance:: '
.::' (

PlayerLeft::( 2
-=::3 5
OnPlayerLeft::6 B
;::B C
};; 	
private?? 
async?? 
void?? 
Window_Loaded?? (
(??( )
object??) /
sender??0 6
,??6 7
RoutedEventArgs??8 G
e??H I
)??I J
{@@ 	
tryAA 
{BB 
boolCC 
successCC 
;CC 
stringDD 
usernameDD 
=DD  !
UserSessionDD" -
.DD- .
UsernameDD. 6
;DD6 7
ifFF 
(FF 
UserSessionFF 
.FF  
IsGuestFF  '
)FF' (
{GG 
successHH 
=HH 
awaitHH #
GameServiceManagerHH$ 6
.HH6 7
InstanceHH7 ?
.HH? @
ClientHH@ F
.HHF G
JoinLobbyAsyncHHG U
(HHU V
UserSessionII #
.II# $
SessionTokenII$ 0
,II0 1

_lobbyCodeII2 <
,II< =
trueII> B
,IIB C
usernameIID L
)IIL M
;IIM N
}JJ 
elseKK 
{LL 
successMM 
=MM 
awaitMM #
GameServiceManagerMM$ 6
.MM6 7
InstanceMM7 ?
.MM? @
ClientMM@ F
.MMF G
JoinLobbyAsyncMMG U
(MMU V
UserSessionNN #
.NN# $
SessionTokenNN$ 0
,NN0 1

_lobbyCodeNN2 <
,NN< =
falseNN> C
,NNC D
nullNNE I
)NNI J
;NNJ K
}OO 
ifQQ 
(QQ 
successQQ 
)QQ 
{RR 
_isConnectedSS  
=SS! "
trueSS# '
;SS' (
ifUU 
(UU 
!UU 
_currentPlayersUU (
.UU( )
AnyUU) ,
(UU, -
pUU- .
=>UU/ 1
pUU2 3
.UU3 4
NameUU4 8
==UU9 ;
usernameUU< D
)UUD E
)UUE F
{VV 
_currentPlayersWW '
.WW' (
AddWW( +
(WW+ ,
newWW, /
LobbyPlayerInfoWW0 ?
{WW@ A
NameWWB F
=WWG H
usernameWWI Q
}WWR S
)WWS T
;WWT U
UpdatePlayerUIXX &
(XX& '
)XX' (
;XX( )
}YY !
OnChatMessageReceived[[ )
([[) *
$str[[* 2
,[[2 3
$"[[4 6
{[[6 7
username[[7 ?
}[[? @
$str[[@ V
"[[V W
,[[W X
true[[Y ]
)[[] ^
;[[^ _
string]] 

successMsg]] %
=]]& '
Lang]]( ,
.]], -+
Lobby_Notification_PlayerJoined]]- L
.]]L M
Contains]]M U
(]]U V
$str]]V [
)]][ \
?^^ 
string^^  
.^^  !
Format^^! '
(^^' (
Lang^^( ,
.^^, -+
Lobby_Notification_PlayerJoined^^- L
,^^L M
username^^N V
)^^V W
:__ 
Lang__ 
.__ +
Lobby_Notification_PlayerJoined__ >
;__> ?
newaa 
CustomMessageBoxaa (
(aa( )
Langaa) -
.aa- . 
Global_Title_Successaa. B
,aaB C

successMsgaaD N
,aaN O
thisaaP T
,aaT U
MessageBoxTypeaaV d
.aad e
Informationaae p
)aap q
.aaq r

ShowDialogaar |
(aa| }
)aa} ~
;aa~ 
}bb 
elsecc 
{dd 
newee 
CustomMessageBoxee (
(ee( )
Langee) -
.ee- .
Global_Title_Erroree. @
,ee@ A
LangeeB F
.eeF G"
Lobby_Error_JoinFailedeeG ]
,ee] ^
thisee_ c
,eec d
MessageBoxTypeeee s
.ees t
Erroreet y
)eey z
.eez {

ShowDialog	ee{ Ö
(
eeÖ Ü
)
eeÜ á
;
eeá à
GoBackToMenuff  
(ff  !
)ff! "
;ff" #
}gg 
}hh 
catchii 
(ii 
	Exceptionii 
exii 
)ii  
{jj 
ExceptionManagerkk  
.kk  !
Handlekk! '
(kk' (
exkk( *
,kk* +
thiskk, 0
,kk0 1
asynckk2 7
(kk8 9
)kk9 :
=>kk; =
{ll 
awaitmm 
LeaveLobbySafemm (
(mm( )
)mm) *
;mm* +
thisnn 
.nn 
Closenn 
(nn 
)nn  
;nn  !
}oo 
)oo 
;oo 
}pp 
}qq 	
privateuu 
voiduu 
OnPlayerListUpdateduu (
(uu( )
LobbyPlayerInfouu) 8
[uu8 9
]uu9 :
playersuu; B
)uuB C
{vv 	

Dispatcherww 
.ww 
Invokeww 
(ww 
(ww 
)ww  
=>ww! #
{xx 
_currentPlayersyy 
=yy  !
playersyy" )
.yy) *
ToListyy* 0
(yy0 1
)yy1 2
;yy2 3
UpdatePlayerUIzz 
(zz 
)zz  
;zz  !
}{{ 
){{ 
;{{ 
}|| 	
private~~ 
void~~ 
UpdatePlayerUI~~ #
(~~# $
)~~$ %
{ 	
if
ÄÄ 
(
ÄÄ 
PlayersListBox
ÄÄ 
==
ÄÄ !
null
ÄÄ" &
)
ÄÄ& '
{
ÅÅ 
return
ÇÇ 
;
ÇÇ 
}
ÉÉ 
PlayersListBox
ÖÖ 
.
ÖÖ 
Items
ÖÖ  
.
ÖÖ  !
Clear
ÖÖ! &
(
ÖÖ& '
)
ÖÖ' (
;
ÖÖ( )
foreach
ÜÜ 
(
ÜÜ 
var
ÜÜ 
player
ÜÜ 
in
ÜÜ  "
_currentPlayers
ÜÜ# 2
)
ÜÜ2 3
{
áá 
PlayersListBox
àà 
.
àà 
Items
àà $
.
àà$ %
Add
àà% (
(
àà( )
player
àà) /
.
àà/ 0
Name
àà0 4
)
àà4 5
;
àà5 6
}
ââ 
}
ää 	
private
åå 
void
åå #
OnChatMessageReceived
åå *
(
åå* +
string
åå+ 1
sender
åå2 8
,
åå8 9
string
åå: @
message
ååA H
,
ååH I
bool
ååJ N
isNotification
ååO ]
)
åå] ^
{
çç 	

Dispatcher
éé 
.
éé 
Invoke
éé 
(
éé 
(
éé 
)
éé  
=>
éé! #
{
èè 
if
êê 
(
êê 
ChatListBox
êê 
!=
êê  "
null
êê# '
)
êê' (
{
ëë 
string
íí 
formattedMessage
íí +
=
íí, -
isNotification
íí. <
?
íí= >
$"
íí? A
$str
ííA E
{
ííE F
message
ííF M
}
ííM N
$str
ííN R
"
ííR S
:
ííT U
$"
ííV X
{
ííX Y
sender
ííY _
}
íí_ `
$str
íí` b
{
ííb c
message
ííc j
}
ííj k
"
íík l
;
ííl m
ChatListBox
ìì 
.
ìì  
Items
ìì  %
.
ìì% &
Add
ìì& )
(
ìì) *
formattedMessage
ìì* :
)
ìì: ;
;
ìì; <
if
ïï 
(
ïï 
ChatListBox
ïï #
.
ïï# $
Items
ïï$ )
.
ïï) *
Count
ïï* /
>
ïï0 1
$num
ïï2 3
)
ïï3 4
{
ññ 
ChatListBox
óó #
.
óó# $
ScrollIntoView
óó$ 2
(
óó2 3
ChatListBox
óó3 >
.
óó> ?
Items
óó? D
[
óóD E
ChatListBox
óóE P
.
óóP Q
Items
óóQ V
.
óóV W
Count
óóW \
-
óó] ^
$num
óó_ `
]
óó` a
)
óóa b
;
óób c
}
òò 
}
ôô 
}
öö 
)
öö 
;
öö 
}
õõ 	
private
°° 
void
°° 
ButtonReady_Click
°° &
(
°°& '
object
°°' -
sender
°°. 4
,
°°4 5
RoutedEventArgs
°°6 E
e
°°F G
)
°°G H
{
¢¢ 	
if
££ 
(
££ 
sender
££ 
is
££ 
Button
££  
btn
££! $
)
££$ %
{
§§ 
btn
•• 
.
•• 
	IsEnabled
•• 
=
•• 
false
••  %
;
••% &
btn
¶¶ 
.
¶¶ 
Content
¶¶ 
=
¶¶ 
$str
¶¶ *
;
¶¶* +
}
ßß 
}
®® 	
private
™™ 
void
™™  
ButtonInvite_Click
™™ '
(
™™' (
object
™™( .
sender
™™/ 5
,
™™5 6
RoutedEventArgs
™™7 F
e
™™G H
)
™™H I
{
´´ 	
var
¨¨ 
inviteDialog
¨¨ 
=
¨¨ 
new
¨¨ " 
InviteFriendDialog
¨¨# 5
(
¨¨5 6

_lobbyCode
¨¨6 @
)
¨¨@ A
;
¨¨A B
NavigationHelper
≠≠ 
.
≠≠ 

ShowDialog
≠≠ '
(
≠≠' (
this
≠≠( ,
,
≠≠, -
inviteDialog
≠≠. :
)
≠≠: ;
;
≠≠; <
}
ÆÆ 	
private
∞∞ 
async
∞∞ 
void
∞∞ )
ButtonSendMessageChat_Click
∞∞ 6
(
∞∞6 7
object
∞∞7 =
sender
∞∞> D
,
∞∞D E
RoutedEventArgs
∞∞F U
e
∞∞V W
)
∞∞W X
{
±± 	
if
≤≤ 
(
≤≤ 
ChatTextBox
≤≤ 
!=
≤≤ 
null
≤≤ #
&&
≤≤$ &
!
≤≤' (
string
≤≤( .
.
≤≤. / 
IsNullOrWhiteSpace
≤≤/ A
(
≤≤A B
ChatTextBox
≤≤B M
.
≤≤M N
Text
≤≤N R
)
≤≤R S
)
≤≤S T
{
≥≥ 
string
¥¥ 
msg
¥¥ 
=
¥¥ 
ChatTextBox
¥¥ (
.
¥¥( )
Text
¥¥) -
;
¥¥- .
ChatTextBox
µµ 
.
µµ 
Text
µµ  
=
µµ! "
string
µµ# )
.
µµ) *
Empty
µµ* /
;
µµ/ 0
try
∑∑ 
{
∏∏ 
await
ππ  
GameServiceManager
ππ ,
.
ππ, -
Instance
ππ- 5
.
ππ5 6
Client
ππ6 <
.
ππ< ="
SendChatMessageAsync
ππ= Q
(
ππQ R
msg
ππR U
)
ππU V
;
ππV W
}
∫∫ 
catch
ªª 
(
ªª 
	Exception
ªª  
ex
ªª! #
)
ªª# $
{
ºº 
ExceptionManager
ΩΩ $
.
ΩΩ$ %
Handle
ΩΩ% +
(
ΩΩ+ ,
ex
ΩΩ, .
,
ΩΩ. /
this
ΩΩ0 4
)
ΩΩ4 5
;
ΩΩ5 6
}
ææ 
}
øø 
}
¿¿ 	
private
¬¬ 
async
¬¬ 
void
¬¬ (
ButtonBackToMainMenu_Click
¬¬ 5
(
¬¬5 6
object
¬¬6 <
sender
¬¬= C
,
¬¬C D
RoutedEventArgs
¬¬E T
e
¬¬U V
)
¬¬V W
{
√√ 	$
ConfirmationMessageBox
ƒƒ "
confirmationBox
ƒƒ# 2
=
ƒƒ3 4
new
ƒƒ5 8$
ConfirmationMessageBox
ƒƒ9 O
(
ƒƒO P
Lang
≈≈ 
.
≈≈ %
Global_Title_LeaveLobby
≈≈ ,
,
≈≈, -
Lang
≈≈. 2
.
≈≈2 3&
Lobby_Message_LeaveLobby
≈≈3 K
,
≈≈K L
this
∆∆ 
,
∆∆ $
ConfirmationMessageBox
∆∆ ,
.
∆∆, -!
ConfirmationBoxType
∆∆- @
.
∆∆@ A
Warning
∆∆A H
)
∆∆H I
;
∆∆I J
if
»» 
(
»» 
confirmationBox
»» 
.
»»  

ShowDialog
»»  *
(
»»* +
)
»»+ ,
==
»»- /
true
»»0 4
)
»»4 5
{
…… 
await
   
LeaveLobbySafe
   $
(
  $ %
)
  % &
;
  & '
GoBackToMenu
ÀÀ 
(
ÀÀ 
)
ÀÀ 
;
ÀÀ 
}
ÃÃ 
}
ÕÕ 	
private
œœ 
void
œœ 
GoBackToMenu
œœ !
(
œœ! "
)
œœ" #
{
–– 	
NavigationHelper
—— 
.
—— 

NavigateTo
—— '
(
——' (
this
——( ,
,
——, -
this
——. 2
.
——2 3
Owner
——3 8
??
——9 ;
new
——< ?
MultiplayerMenu
——@ O
(
——O P
)
——P Q
)
——Q R
;
——R S
}
““ 	
private
ÿÿ 
void
ÿÿ 
OnGameStarted
ÿÿ "
(
ÿÿ" #
List
ÿÿ# '
<
ÿÿ' (
CardInfo
ÿÿ( 0
>
ÿÿ0 1
cards
ÿÿ2 7
)
ÿÿ7 8
{
ŸŸ 	

Dispatcher
⁄⁄ 
.
⁄⁄ 
Invoke
⁄⁄ 
(
⁄⁄ 
(
⁄⁄ 
)
⁄⁄  
=>
⁄⁄! #
{
€€ 
UnsubscribeEvents
‹‹ !
(
‹‹! "
)
‹‹" #
;
‹‹# $
_isGameStarting
›› 
=
››  !
true
››" &
;
››& '
var
ﬂﬂ 

gameWindow
ﬂﬂ 
=
ﬂﬂ  
new
ﬂﬂ! $!
PlayGameMultiplayer
ﬂﬂ% 8
(
ﬂﬂ8 9
cards
ﬂﬂ9 >
,
ﬂﬂ> ?
_currentPlayers
ﬂﬂ@ O
)
ﬂﬂO P
;
ﬂﬂP Q
if
·· 
(
·· 
this
·· 
.
·· 
Owner
·· 
!=
·· !
null
··" &
)
··& '
{
‚‚ 

gameWindow
„„ 
.
„„ 
Owner
„„ $
=
„„% &
this
„„' +
.
„„+ ,
Owner
„„, 1
;
„„1 2
}
‰‰ 
NavigationHelper
ÊÊ  
.
ÊÊ  !

NavigateTo
ÊÊ! +
(
ÊÊ+ ,
this
ÊÊ, 0
,
ÊÊ0 1

gameWindow
ÊÊ2 <
)
ÊÊ< =
;
ÊÊ= >
}
ÁÁ 
)
ÁÁ 
;
ÁÁ 
}
ËË 	
private
ÍÍ 
void
ÍÍ 
OnPlayerLeft
ÍÍ !
(
ÍÍ! "
string
ÍÍ" (
name
ÍÍ) -
)
ÍÍ- .
{
ÎÎ 	

Dispatcher
ÏÏ 
.
ÏÏ 
Invoke
ÏÏ 
(
ÏÏ 
(
ÏÏ 
)
ÏÏ  
=>
ÏÏ! #
{
ÌÌ 
var
ÓÓ 
player
ÓÓ 
=
ÓÓ 
_currentPlayers
ÓÓ ,
.
ÓÓ, -
FirstOrDefault
ÓÓ- ;
(
ÓÓ; <
x
ÓÓ< =
=>
ÓÓ> @
x
ÓÓA B
.
ÓÓB C
Name
ÓÓC G
==
ÓÓH J
name
ÓÓK O
)
ÓÓO P
;
ÓÓP Q
if
ÔÔ 
(
ÔÔ 
player
ÔÔ 
!=
ÔÔ 
null
ÔÔ "
)
ÔÔ" #
{
 
_currentPlayers
ÒÒ #
.
ÒÒ# $
Remove
ÒÒ$ *
(
ÒÒ* +
player
ÒÒ+ 1
)
ÒÒ1 2
;
ÒÒ2 3
UpdatePlayerUI
ÚÚ "
(
ÚÚ" #
)
ÚÚ# $
;
ÚÚ$ %#
OnChatMessageReceived
ÛÛ )
(
ÛÛ) *
$str
ÛÛ* 2
,
ÛÛ2 3
$"
ÛÛ4 6
{
ÛÛ6 7
name
ÛÛ7 ;
}
ÛÛ; <
$str
ÛÛ< L
"
ÛÛL M
,
ÛÛM N
true
ÛÛO S
)
ÛÛS T
;
ÛÛT U
}
ÙÙ 
}
ıı 
)
ıı 
;
ıı 
}
ˆˆ 	
	protected
¯¯ 
override
¯¯ 
async
¯¯  
void
¯¯! %
OnClosed
¯¯& .
(
¯¯. /
	EventArgs
¯¯/ 8
e
¯¯9 :
)
¯¯: ;
{
˘˘ 	
UnsubscribeEvents
˙˙ 
(
˙˙ 
)
˙˙ 
;
˙˙  
if
¸¸ 
(
¸¸ 
!
¸¸ 
_isGameStarting
¸¸  
)
¸¸  !
{
˝˝ 
await
˛˛ 
LeaveLobbySafe
˛˛ $
(
˛˛$ %
)
˛˛% &
;
˛˛& '
if
ÄÄ 
(
ÄÄ 
this
ÄÄ 
.
ÄÄ 
Owner
ÄÄ 
!=
ÄÄ !
null
ÄÄ" &
&&
ÄÄ' )
Application
ÄÄ* 5
.
ÄÄ5 6
Current
ÄÄ6 =
.
ÄÄ= >

MainWindow
ÄÄ> H
!=
ÄÄI K
this
ÄÄL P
.
ÄÄP Q
Owner
ÄÄQ V
)
ÄÄV W
{
ÅÅ 
this
ÇÇ 
.
ÇÇ 
Owner
ÇÇ 
.
ÇÇ 
Show
ÇÇ #
(
ÇÇ# $
)
ÇÇ$ %
;
ÇÇ% &
}
ÉÉ 
}
ÑÑ 
base
ÜÜ 
.
ÜÜ 
OnClosed
ÜÜ 
(
ÜÜ 
e
ÜÜ 
)
ÜÜ 
;
ÜÜ 
}
áá 	
private
ââ 
async
ââ 
Task
ââ 
LeaveLobbySafe
ââ )
(
ââ) *
)
ââ* +
{
ää 	
if
ãã 
(
ãã 
_isConnected
ãã 
)
ãã 
{
åå 
try
çç 
{
éé 
await
èè  
GameServiceManager
èè ,
.
èè, -
Instance
èè- 5
.
èè5 6
Client
èè6 <
.
èè< =
LeaveLobbyAsync
èè= L
(
èèL M
)
èèM N
;
èèN O
}
êê 
catch
ëë 
(
ëë 
	Exception
ëë  
ex
ëë! #
)
ëë# $
{
íí 
Debug
ìì 
.
ìì 
	WriteLine
ìì #
(
ìì# $
$"
ìì$ &
$str
ìì& F
{
ììF G
ex
ììG I
.
ììI J
Message
ììJ Q
}
ììQ R
"
ììR S
)
ììS T
;
ììT U
}
îî 
finally
ïï 
{
ññ 
_isConnected
óó  
=
óó! "
false
óó# (
;
óó( )
}
òò 
}
ôô 
}
öö 	
}
ùù 
}ûû ú¬
9C:\MemoryGame\Client\Client\Views\Lobby\HostLobby.xaml.cs
	namespace 	
Client
 
. 
Views 
. 
Lobby 
{ 
public 

partial 
class 
	HostLobby "
:# $
Window% +
{ 
private 
readonly 
string 

_lobbyCode  *
;* +
private 
bool 
_isConnected !
=" #
false$ )
;) *
private 
bool 
_isGameStarting $
=% &
false' ,
;, -
private 
List 
< 
LobbyPlayerInfo $
>$ %
_currentPlayers& 5
=6 7
new8 ;
List< @
<@ A
LobbyPlayerInfoA P
>P Q
(Q R
)R S
;S T
private 
int 
_selectedCardCount &
=' (
GameConstants) 6
.6 7
DefaultCardCount7 G
;G H
private 
int 
_secondsPerTurn #
=$ %
GameConstants& 3
.3 4"
DefaultTurnTimeSeconds4 J
;J K
public"" 
	HostLobby"" 
("" 
)"" 
{## 	
InitializeComponent$$ 
($$  
)$$  !
;$$! "

_lobbyCode&& 
=&& 
ClientHelper&& %
.&&% &
GenerateGameCode&&& 6
(&&6 7
)&&7 8
;&&8 9
if(( 
((( 
LabelGameCode(( 
!=((  
null((! %
)((% &
{)) 
LabelGameCode** 
.** 
Content** %
=**& '

_lobbyCode**( 2
;**2 3
}++ 
if,, 
(,, 
LabelTimerValue,, 
!=,,  "
null,,# '
),,' (
{-- 
LabelTimerValue.. 
...  
Content..  '
=..( )
_secondsPerTurn..* 9
...9 :
ToString..: B
(..B C
)..C D
;..D E
}// 
if00 
(00 !
ComboBoxNumberOfCards00 %
!=00& (
null00) -
)00- .
{11 !
ComboBoxNumberOfCards22 %
.22% &
SelectedIndex22& 3
=224 5
$num226 7
;227 8
}33 
ConfigureEvents44 
(44 
)44 
;44 
}55 	
private99 
void99 
ConfigureEvents99 $
(99$ %
)99% &
{:: 	
GameServiceManager;; 
.;; 
Instance;; '
.;;' (
PlayerListUpdated;;( 9
+=;;: <
OnPlayerListUpdated;;= P
;;;P Q
GameServiceManager<< 
.<< 
Instance<< '
.<<' (
GameStarted<<( 3
+=<<4 6
OnGameStarted<<7 D
;<<D E
GameServiceManager== 
.== 
Instance== '
.==' (
ChatMessageReceived==( ;
+===< >!
OnChatMessageReceived==? T
;==T U
GameServiceManager>> 
.>> 
Instance>> '
.>>' (

PlayerLeft>>( 2
+=>>3 5
OnPlayerLeft>>6 B
;>>B C
}?? 	
privateAA 
voidAA 
UnsubscribeEventsAA &
(AA& '
)AA' (
{BB 	
GameServiceManagerCC 
.CC 
InstanceCC '
.CC' (
PlayerListUpdatedCC( 9
-=CC: <
OnPlayerListUpdatedCC= P
;CCP Q
GameServiceManagerDD 
.DD 
InstanceDD '
.DD' (
GameStartedDD( 3
-=DD4 6
OnGameStartedDD7 D
;DDD E
GameServiceManagerEE 
.EE 
InstanceEE '
.EE' (
ChatMessageReceivedEE( ;
-=EE< >!
OnChatMessageReceivedEE? T
;EET U
GameServiceManagerFF 
.FF 
InstanceFF '
.FF' (

PlayerLeftFF( 2
-=FF3 5
OnPlayerLeftFF6 B
;FFB C
}GG 	
privateMM 
asyncMM 
voidMM 
Window_LoadedMM (
(MM( )
objectMM) /
senderMM0 6
,MM6 7
RoutedEventArgsMM8 G
eMMH I
)MMI J
{NN 	
tryOO 
{PP 
boolQQ 
successQQ 
=QQ 
awaitQQ $
GameServiceManagerQQ% 7
.QQ7 8
InstanceQQ8 @
.QQ@ A
ClientQQA G
.QQG H
CreateLobbyAsyncQQH X
(QQX Y
UserSessionRR 
.RR  
SessionTokenRR  ,
,RR, -

_lobbyCodeRR. 8
)RR8 9
;RR9 :
ifTT 
(TT 
successTT 
)TT 
{UU 
_isConnectedVV  
=VV! "
trueVV# '
;VV' (
stringWW 
myNameWW !
=WW" #
UserSessionWW$ /
.WW/ 0
UsernameWW0 8
;WW8 9
ifYY 
(YY 
!YY 
_currentPlayersYY (
.YY( )
AnyYY) ,
(YY, -
pYY- .
=>YY/ 1
pYY2 3
.YY3 4
NameYY4 8
==YY9 ;
myNameYY< B
)YYB C
)YYC D
{ZZ 
_currentPlayers[[ '
.[[' (
Add[[( +
([[+ ,
new[[, /
LobbyPlayerInfo[[0 ?
{[[@ A
Name[[B F
=[[G H
myName[[I O
}[[P Q
)[[Q R
;[[R S
UpdatePlayerUI\\ &
(\\& '
)\\' (
;\\( )
}]] 
new__ 
CustomMessageBox__ (
(__( )
Lang__) -
.__- . 
Global_Title_Success__. B
,__B C
Lang__D H
.__H I'
Lobby_Message_CreateSuccess__I d
,__d e
this__f j
,__j k
MessageBoxType__l z
.__z {
Success	__{ Ç
)
__Ç É
.
__É Ñ

ShowDialog
__Ñ é
(
__é è
)
__è ê
;
__ê ë
}`` 
elseaa 
{bb 
newcc 
CustomMessageBoxcc (
(cc( )
Langcc) -
.cc- .
Global_Title_Errorcc. @
,cc@ A
LangccB F
.ccF G(
HostLobby_Error_CreateFailedccG c
,ccc d
thiscce i
,cci j
MessageBoxTypecck y
.ccy z
Errorccz 
)	cc Ä
.
ccÄ Å

ShowDialog
ccÅ ã
(
ccã å
)
ccå ç
;
ccç é
GoBackToMenudd  
(dd  !
)dd! "
;dd" #
}ee 
}ff 
catchgg 
(gg 
	Exceptiongg 
exgg 
)gg  
{hh 
ExceptionManagerii  
.ii  !
Handleii! '
(ii' (
exii( *
,ii* +
thisii, 0
,ii0 1
asyncii2 7
(ii8 9
)ii9 :
=>ii; =
{jj 
awaitkk 
LeaveLobbySafekk (
(kk( )
)kk) *
;kk* +
GoBackToMenull  
(ll  !
)ll! "
;ll" #
}mm 
)mm 
;mm 
}nn 
}oo 	
privateuu 
voiduu 
OnPlayerListUpdateduu (
(uu( )
LobbyPlayerInfouu) 8
[uu8 9
]uu9 :
playersuu; B
)uuB C
{vv 	
ifww 
(ww 
_isGameStartingww 
)ww  
{xx 
returnyy 
;yy 
}zz 

Dispatcher|| 
.|| 
Invoke|| 
(|| 
(|| 
)||  
=>||! #
{}} 
if~~ 
(~~ 
_isGameStarting~~ #
||~~$ &
!~~' (
this~~( ,
.~~, -
IsLoaded~~- 5
)~~5 6
{ 
return
ÄÄ 
;
ÄÄ 
}
ÅÅ 
_currentPlayers
ÉÉ 
=
ÉÉ  !
players
ÉÉ" )
.
ÉÉ) *
ToList
ÉÉ* 0
(
ÉÉ0 1
)
ÉÉ1 2
;
ÉÉ2 3
UpdatePlayerUI
ÑÑ 
(
ÑÑ 
)
ÑÑ  
;
ÑÑ  !
}
ÖÖ 
)
ÖÖ 
;
ÖÖ 
}
ÜÜ 	
private
àà 
void
àà 
UpdatePlayerUI
àà #
(
àà# $
)
àà$ %
{
ââ 	
if
ää 
(
ää 
PlayersListBox
ää 
==
ää !
null
ää" &
)
ää& '
{
ãã 
return
åå 
;
åå 
}
çç 
PlayersListBox
èè 
.
èè 
ItemsSource
èè &
=
èè' (
_currentPlayers
èè) 8
.
êê 
Select
êê 
(
êê 
player
êê 
=>
êê !
player
êê" (
.
êê( )
Name
êê) -
==
êê. 0
UserSession
êê1 <
.
êê< =
Username
êê= E
?
êêF G
$"
êêH J
{
êêJ K
player
êêK Q
.
êêQ R
Name
êêR V
}
êêV W
$str
êêW ^
"
êê^ _
:
êê` a
player
êêb h
.
êêh i
Name
êêi m
)
êêm n
.
ëë 
ToList
ëë 
(
ëë 
)
ëë 
;
ëë 
}
íí 	
private
îî 
void
îî #
OnChatMessageReceived
îî *
(
îî* +
string
îî+ 1
sender
îî2 8
,
îî8 9
string
îî: @
message
îîA H
,
îîH I
bool
îîJ N
isNotification
îîO ]
)
îî] ^
{
ïï 	
if
ññ 
(
ññ 
_isGameStarting
ññ 
)
ññ  
{
óó 
return
òò 
;
òò 
}
ôô 

Dispatcher
õõ 
.
õõ 
Invoke
õõ 
(
õõ 
(
õõ 
)
õõ  
=>
õõ! #
{
úú 
if
ùù 
(
ùù 
_isGameStarting
ùù #
||
ùù$ &
!
ùù' (
this
ùù( ,
.
ùù, -
IsLoaded
ùù- 5
)
ùù5 6
{
ûû 
return
üü 
;
üü 
}
†† 
if
¢¢ 
(
¢¢ 
ChatListBox
¢¢ 
!=
¢¢  "
null
¢¢# '
)
¢¢' (
{
££ 
string
§§ 
formattedMessage
§§ +
=
§§, -
isNotification
§§. <
?
§§= >
$"
§§? A
$str
§§A E
{
§§E F
message
§§F M
}
§§M N
$str
§§N R
"
§§R S
:
§§T U
$"
§§V X
{
§§X Y
sender
§§Y _
}
§§_ `
$str
§§` b
{
§§b c
message
§§c j
}
§§j k
"
§§k l
;
§§l m
ChatListBox
•• 
.
••  
Items
••  %
.
••% &
Add
••& )
(
••) *
formattedMessage
••* :
)
••: ;
;
••; <
if
ßß 
(
ßß 
ChatListBox
ßß #
.
ßß# $
Items
ßß$ )
.
ßß) *
Count
ßß* /
>
ßß0 1
$num
ßß2 3
)
ßß3 4
{
®® 
ChatListBox
©© #
.
©©# $
ScrollIntoView
©©$ 2
(
©©2 3
ChatListBox
©©3 >
.
©©> ?
Items
©©? D
[
©©D E
ChatListBox
©©E P
.
©©P Q
Items
©©Q V
.
©©V W
Count
©©W \
-
©©] ^
$num
©©_ `
]
©©` a
)
©©a b
;
©©b c
}
™™ 
}
´´ 
}
¨¨ 
)
¨¨ 
;
¨¨ 
}
≠≠ 	
private
≥≥ 
void
≥≥ $
ButtonStartMatch_Click
≥≥ +
(
≥≥+ ,
object
≥≥, 2
sender
≥≥3 9
,
≥≥9 :
RoutedEventArgs
≥≥; J
e
≥≥K L
)
≥≥L M
{
¥¥ 	
if
µµ 
(
µµ 
!
µµ 
_isConnected
µµ 
)
µµ 
{
∂∂ 
return
∑∑ 
;
∑∑ 
}
∏∏ 
if
∫∫ 
(
∫∫ 
_currentPlayers
∫∫ 
.
∫∫  
Count
∫∫  %
<
∫∫& '
GameConstants
∫∫( 5
.
∫∫5 6
MinPlayersToPlay
∫∫6 F
)
∫∫F G
{
ªª 

MessageBox
ºº 
.
ºº 
Show
ºº 
(
ºº  
$str
ºº  E
,
ººE F
$str
ººG U
,
ººU V
MessageBoxButton
ººW g
.
ººg h
OK
ººh j
,
ººj k
MessageBoxImage
ººl {
.
ºº{ |
Warningºº| É
)ººÉ Ñ
;ººÑ Ö
return
ΩΩ 
;
ΩΩ 
}
ææ 
try
¿¿ 
{
¡¡ 
var
¬¬ 
settings
¬¬ 
=
¬¬ 
new
¬¬ "
GameSettings
¬¬# /
{
√√ 
	CardCount
ƒƒ 
=
ƒƒ  
_selectedCardCount
ƒƒ  2
,
ƒƒ2 3
TurnTimeSeconds
≈≈ #
=
≈≈$ %
_secondsPerTurn
≈≈& 5
}
∆∆ 
;
∆∆ 
if
»» 
(
»» 
ButtonStart
»» 
!=
»»  "
null
»»# '
)
»»' (
{
…… 
ButtonStart
   
.
    
	IsEnabled
    )
=
  * +
false
  , 1
;
  1 2
}
ÀÀ  
GameServiceManager
ÕÕ "
.
ÕÕ" #
Instance
ÕÕ# +
.
ÕÕ+ ,
Client
ÕÕ, 2
.
ÕÕ2 3
	StartGame
ÕÕ3 <
(
ÕÕ< =
settings
ÕÕ= E
)
ÕÕE F
;
ÕÕF G
}
ŒŒ 
catch
œœ 
(
œœ 
	Exception
œœ 
ex
œœ 
)
œœ  
{
–– 
if
—— 
(
—— 
ButtonStart
—— 
!=
——  "
null
——# '
)
——' (
{
““ 
ButtonStart
”” 
.
””  
	IsEnabled
””  )
=
””* +
true
””, 0
;
””0 1
}
‘‘ 
ExceptionManager
’’  
.
’’  !
Handle
’’! '
(
’’' (
ex
’’( *
,
’’* +
this
’’, 0
)
’’0 1
;
’’1 2
}
÷÷ 
}
◊◊ 	
private
ŸŸ 
void
ŸŸ 
OnGameStarted
ŸŸ "
(
ŸŸ" #
List
ŸŸ# '
<
ŸŸ' (
CardInfo
ŸŸ( 0
>
ŸŸ0 1
cards
ŸŸ2 7
)
ŸŸ7 8
{
⁄⁄ 	

Dispatcher
€€ 
.
€€ 
Invoke
€€ 
(
€€ 
(
€€ 
)
€€  
=>
€€! #
{
‹‹ 
_isGameStarting
›› 
=
››  !
true
››" &
;
››& '
UnsubscribeEvents
ﬂﬂ !
(
ﬂﬂ! "
)
ﬂﬂ" #
;
ﬂﬂ# $
var
·· 
multiplayerGame
·· #
=
··$ %
new
··& )!
PlayGameMultiplayer
··* =
(
··= >
cards
··> C
,
··C D
_currentPlayers
··E T
)
··T U
;
··U V
if
„„ 
(
„„ 
this
„„ 
.
„„ 
Owner
„„ 
!=
„„ !
null
„„" &
)
„„& '
{
‰‰ 
multiplayerGame
ÂÂ #
.
ÂÂ# $
Owner
ÂÂ$ )
=
ÂÂ* +
this
ÂÂ, 0
.
ÂÂ0 1
Owner
ÂÂ1 6
;
ÂÂ6 7
}
ÊÊ 
NavigationHelper
ËË  
.
ËË  !

NavigateTo
ËË! +
(
ËË+ ,
this
ËË, 0
,
ËË0 1
multiplayerGame
ËË2 A
)
ËËA B
;
ËËB C
}
ÈÈ 
)
ÈÈ 
;
ÈÈ 
}
ÍÍ 	
private
 
void
 
OnPlayerLeft
 !
(
! "
string
" (

playerName
) 3
)
3 4
{
ÒÒ 	
if
ÚÚ 
(
ÚÚ 
_isGameStarting
ÚÚ 
)
ÚÚ  
{
ÛÛ 
return
ÙÙ 
;
ÙÙ 
}
ıı 

Dispatcher
˜˜ 
.
˜˜ 
Invoke
˜˜ 
(
˜˜ 
(
˜˜ 
)
˜˜  
=>
˜˜! #
{
¯¯ 
if
˘˘ 
(
˘˘ 
_isGameStarting
˘˘ #
||
˘˘$ &
!
˘˘' (
this
˘˘( ,
.
˘˘, -
IsLoaded
˘˘- 5
)
˘˘5 6
{
˙˙ 
return
˚˚ 
;
˚˚ 
}
¸¸ 
var
˛˛ 
playerToRemove
˛˛ "
=
˛˛# $
_currentPlayers
˛˛% 4
.
˛˛4 5
FirstOrDefault
˛˛5 C
(
˛˛C D
p
˛˛D E
=>
˛˛F H
p
˛˛I J
.
˛˛J K
Name
˛˛K O
==
˛˛P R

playerName
˛˛S ]
)
˛˛] ^
;
˛˛^ _
if
ˇˇ 
(
ˇˇ 
playerToRemove
ˇˇ "
!=
ˇˇ# %
null
ˇˇ& *
)
ˇˇ* +
{
ÄÄ 
_currentPlayers
ÅÅ #
.
ÅÅ# $
Remove
ÅÅ$ *
(
ÅÅ* +
playerToRemove
ÅÅ+ 9
)
ÅÅ9 :
;
ÅÅ: ;
UpdatePlayerUI
ÇÇ "
(
ÇÇ" #
)
ÇÇ# $
;
ÇÇ$ %#
OnChatMessageReceived
ÉÉ )
(
ÉÉ) *
$str
ÉÉ* 2
,
ÉÉ2 3
$"
ÉÉ4 6
{
ÉÉ6 7

playerName
ÉÉ7 A
}
ÉÉA B
$str
ÉÉB R
"
ÉÉR S
,
ÉÉS T
true
ÉÉU Y
)
ÉÉY Z
;
ÉÉZ [
}
ÑÑ 
}
ÖÖ 
)
ÖÖ 
;
ÖÖ 
}
ÜÜ 	
private
àà 
async
àà 
void
àà +
ButtonSendMessageToChat_Click
àà 8
(
àà8 9
object
àà9 ?
sender
àà@ F
,
ààF G
RoutedEventArgs
ààH W
e
ààX Y
)
ààY Z
{
ââ 	
if
ää 
(
ää 
ChatTextBox
ää 
!=
ää 
null
ää #
&&
ää$ &
!
ää' (
string
ää( .
.
ää. / 
IsNullOrWhiteSpace
ää/ A
(
ääA B
ChatTextBox
ääB M
.
ääM N
Text
ääN R
)
ääR S
)
ääS T
{
ãã 
string
åå 
msg
åå 
=
åå 
ChatTextBox
åå (
.
åå( )
Text
åå) -
;
åå- .
ChatTextBox
çç 
.
çç 
Text
çç  
=
çç! "
string
çç# )
.
çç) *
Empty
çç* /
;
çç/ 0
try
èè 
{
êê 
await
ëë  
GameServiceManager
ëë ,
.
ëë, -
Instance
ëë- 5
.
ëë5 6
Client
ëë6 <
.
ëë< ="
SendChatMessageAsync
ëë= Q
(
ëëQ R
msg
ëëR U
)
ëëU V
;
ëëV W
}
íí 
catch
ìì 
(
ìì 
	Exception
ìì  
ex
ìì! #
)
ìì# $
{
îî 
Debug
ïï 
.
ïï 
	WriteLine
ïï #
(
ïï# $
$"
ïï$ &
$str
ïï& 2
{
ïï2 3
ex
ïï3 5
.
ïï5 6
Message
ïï6 =
}
ïï= >
"
ïï> ?
)
ïï? @
;
ïï@ A
}
ññ 
}
óó 
}
òò 	
private
öö 
async
öö 
void
öö $
ButtonBackToMenu_Click
öö 1
(
öö1 2
object
öö2 8
sender
öö9 ?
,
öö? @
RoutedEventArgs
ööA P
e
ööQ R
)
ööR S
{
õõ 	
var
úú 
confirmationBox
úú 
=
úú  !
new
úú" %$
ConfirmationMessageBox
úú& <
(
úú< =
Lang
ùù 
.
ùù %
Global_Title_LeaveLobby
ùù ,
,
ùù, -
Lang
ùù. 2
.
ùù2 3*
HostLobby_Message_LeaveLobby
ùù3 O
,
ùùO P
this
ûû 
,
ûû $
ConfirmationMessageBox
ûû ,
.
ûû, -!
ConfirmationBoxType
ûû- @
.
ûû@ A
Warning
ûûA H
)
ûûH I
;
ûûI J
if
†† 
(
†† 
confirmationBox
†† 
.
††  

ShowDialog
††  *
(
††* +
)
††+ ,
==
††- /
true
††0 4
)
††4 5
{
°° 
await
¢¢ 
LeaveLobbySafe
¢¢ $
(
¢¢$ %
)
¢¢% &
;
¢¢& '
GoBackToMenu
££ 
(
££ 
)
££ 
;
££ 
}
§§ 
}
•• 	
private
ßß 
void
ßß 
GoBackToMenu
ßß !
(
ßß! "
)
ßß" #
{
®® 	
NavigationHelper
©© 
.
©© 

NavigateTo
©© '
(
©©' (
this
©©( ,
,
©©, -
this
©©. 2
.
©©2 3
Owner
©©3 8
??
©©9 ;
new
©©< ?
MultiplayerMenu
©©@ O
(
©©O P
)
©©P Q
)
©©Q R
;
©©R S
}
™™ 	
private
∞∞ 
void
∞∞ 4
&ComboBoxNumberOfCards_SelectionChanged
∞∞ ;
(
∞∞; <
object
∞∞< B
sender
∞∞C I
,
∞∞I J'
SelectionChangedEventArgs
∞∞K d
e
∞∞e f
)
∞∞f g
{
±± 	
if
≤≤ 
(
≤≤ #
ComboBoxNumberOfCards
≤≤ %
.
≤≤% &
SelectedItem
≤≤& 2
is
≤≤3 5
ComboBoxItem
≤≤6 B
item
≤≤C G
&&
≤≤H J
int
≥≥ 
.
≥≥ 
TryParse
≥≥ 
(
≥≥ 
item
≥≥ !
.
≥≥! "
Content
≥≥" )
.
≥≥) *
ToString
≥≥* 2
(
≥≥2 3
)
≥≥3 4
,
≥≥4 5
out
≥≥6 9
int
≥≥: =
value
≥≥> C
)
≥≥C D
)
≥≥D E
{
¥¥  
_selectedCardCount
µµ "
=
µµ# $
value
µµ% *
;
µµ* +
}
∂∂ 
}
∑∑ 	
private
ππ 
void
ππ /
!SliderSecondsPerTurn_ValueChanged
ππ 6
(
ππ6 7
object
ππ7 =
sender
ππ> D
,
ππD E,
RoutedPropertyChangedEventArgs
ππF d
<
ππd e
double
ππe k
>
ππk l
e
ππm n
)
ππn o
{
∫∫ 	
_secondsPerTurn
ªª 
=
ªª 
(
ªª 
int
ªª "
)
ªª" #
e
ªª# $
.
ªª$ %
NewValue
ªª% -
;
ªª- .
if
ºº 
(
ºº 
LabelTimerValue
ºº 
!=
ºº  "
null
ºº# '
)
ºº' (
{
ΩΩ 
LabelTimerValue
ææ 
.
ææ  
Content
ææ  '
=
ææ( )
_secondsPerTurn
ææ* 9
.
ææ9 :
ToString
ææ: B
(
ææB C
)
ææC D
;
ææD E
}
øø 
}
¿¿ 	
	protected
∆∆ 
override
∆∆ 
async
∆∆  
void
∆∆! %
OnClosed
∆∆& .
(
∆∆. /
	EventArgs
∆∆/ 8
e
∆∆9 :
)
∆∆: ;
{
«« 	
UnsubscribeEvents
»» 
(
»» 
)
»» 
;
»»  
if
   
(
   
!
   
_isGameStarting
    
)
    !
{
ÀÀ 
await
ÃÃ 
LeaveLobbySafe
ÃÃ $
(
ÃÃ$ %
)
ÃÃ% &
;
ÃÃ& '
if
ŒŒ 
(
ŒŒ 
this
ŒŒ 
.
ŒŒ 
Owner
ŒŒ 
!=
ŒŒ !
null
ŒŒ" &
&&
ŒŒ' )
Application
ŒŒ* 5
.
ŒŒ5 6
Current
ŒŒ6 =
.
ŒŒ= >

MainWindow
ŒŒ> H
!=
ŒŒI K
this
ŒŒL P
.
ŒŒP Q
Owner
ŒŒQ V
)
ŒŒV W
{
œœ 
this
–– 
.
–– 
Owner
–– 
.
–– 
Show
–– #
(
––# $
)
––$ %
;
––% &
}
—— 
}
““ 
base
‘‘ 
.
‘‘ 
OnClosed
‘‘ 
(
‘‘ 
e
‘‘ 
)
‘‘ 
;
‘‘ 
}
’’ 	
private
◊◊ 
async
◊◊ 
Task
◊◊ 
LeaveLobbySafe
◊◊ )
(
◊◊) *
)
◊◊* +
{
ÿÿ 	
if
ŸŸ 
(
ŸŸ 
_isConnected
ŸŸ 
)
ŸŸ 
{
⁄⁄ 
try
€€ 
{
‹‹ 
await
››  
GameServiceManager
›› ,
.
››, -
Instance
››- 5
.
››5 6
Client
››6 <
.
››< =
LeaveLobbyAsync
››= L
(
››L M
)
››M N
;
››N O
}
ﬁﬁ 
catch
ﬂﬂ 
(
ﬂﬂ 
	Exception
ﬂﬂ  
ex
ﬂﬂ! #
)
ﬂﬂ# $
{
‡‡ 
Debug
·· 
.
·· 
	WriteLine
·· #
(
··# $
$"
··$ &
$str
··& F
{
··F G
ex
··G I
.
··I J
Message
··J Q
}
··Q R
"
··R S
)
··S T
;
··T U
}
‚‚ 
finally
„„ 
{
‰‰ 
_isConnected
ÂÂ  
=
ÂÂ! "
false
ÂÂ# (
;
ÂÂ( )
}
ÊÊ 
}
ÁÁ 
}
ËË 	
}
ÎÎ 
}ÏÏ Û*
EC:\MemoryGame\Client\Client\Views\Controls\InviteFriendDialog.xaml.cs
	namespace 	
Client
 
. 
Views 
. 
Controls 
{		 
public

 

partial

 
class

 
InviteFriendDialog

 +
:

, -
Window

. 4
{ 
private 
readonly 
string 

_lobbyCode  *
;* +
public 
InviteFriendDialog !
(! "
string" (
	lobbyCode) 2
)2 3
{ 	
InitializeComponent 
(  
)  !
;! "

_lobbyCode 
= 
	lobbyCode "
;" #
} 	
private 
void 
ButtonSend_Click %
(% &
object& ,
sender- 3
,3 4
RoutedEventArgs5 D
eE F
)F G
{ 	
string 
email 
= 
TextBoxEmail '
.' (
Text( ,
., -
Trim- 1
(1 2
)2 3
;3 4
if 
( 
string 
. 
IsNullOrEmpty $
($ %
email% *
)* +
)+ ,
{ 
new 
CustomMessageBox $
($ %
Lang 
. 
Global_Title_Error +
,+ ,
Lang 
. 1
%InviteFriendDialog_Message_EnterEmail >
,> ?
this 
, 
MessageBoxType (
.( )
Warning) 0
)0 1
.1 2

ShowDialog2 <
(< =
)= >
;> ?
return 
; 
} 
	SendEmail   
(   
email   
)   
;   
this!! 
.!! 
Close!! 
(!! 
)!! 
;!! 
}"" 	
private$$ 
void$$ 
	SendEmail$$ 
($$ 
string$$ %
targetEmail$$& 1
)$$1 2
{%% 	
try&& 
{'' 
string(( 
subject(( 
=((  
Lang((! %
.((% &1
%InviteFriendDialog_Title_SubjectEmail((& K
;((K L
string)) 
body)) 
=)) 
Lang)) "
.))" #0
$InviteFriendDialog_Message_BodyEmail))# G
+))H I
$"))J L
{))L M

_lobbyCode))M W
}))W X
"))X Y
;))Y Z
string** 
	mailtoUri**  
=**! "
$"**# %
$str**% ,
{**, -
targetEmail**- 8
}**8 9
$str**9 B
{**B C
Uri**C F
.**F G
EscapeDataString**G W
(**W X
subject**X _
)**_ `
}**` a
$str**a g
{**g h
Uri**h k
.**k l
EscapeDataString**l |
(**| }
body	**} Å
)
**Å Ç
}
**Ç É
"
**É Ñ
;
**Ñ Ö
Process++ 
.++ 
Start++ 
(++ 
new++ !
ProcessStartInfo++" 2
(++2 3
	mailtoUri++3 <
)++< =
{++> ?
UseShellExecute++@ O
=++P Q
true++R V
}++W X
)++X Y
;++Y Z
},, 
catch-- 
(-- 
System-- 
.-- 
ComponentModel-- (
.--( )
Win32Exception--) 7
)--7 8
{.. 
new// 
CustomMessageBox// $
(//$ %
Lang00 
.00 
Global_Title_Error00 +
,00+ ,
Lang11 
.11 2
&InviteFriendDialog_Label_ErrorAppEmail11 ?
,11? @
this22 
,22 
MessageBoxType22 (
.22( )
Error22) .
)22. /
.22/ 0

ShowDialog220 :
(22: ;
)22; <
;22< =
}33 
catch44 
(44 
	Exception44 
ex44 
)44  
{55 
Debug66 
.66 
	WriteLine66 
(66  
$"66  "
$str66" 5
{665 6
ex666 8
.668 9
Message669 @
}66@ A
"66A B
)66B C
;66C D
new77 
CustomMessageBox77 $
(77$ %
Lang88 
.88 
Global_Title_Error88 +
,88+ ,
Lang99 
.99 '
Global_ServiceError_Unknown99 4
,994 5
this:: 
,:: 
MessageBoxType:: (
.::( )
Error::) .
)::. /
.::/ 0

ShowDialog::0 :
(::: ;
)::; <
;::< =
};; 
}<< 	
private>> 
void>> 
ButtonCancel_Click>> '
(>>' (
object>>( .
sender>>/ 5
,>>5 6
RoutedEventArgs>>7 F
e>>G H
)>>H I
{?? 	
this@@ 
.@@ 
Close@@ 
(@@ 
)@@ 
;@@ 
}AA 	
privateCC 
voidCC &
Window_MouseLeftButtonDownCC /
(CC/ 0
objectCC0 6
senderCC7 =
,CC= > 
MouseButtonEventArgsCC? S
eCCT U
)CCU V
{DD 	
DragMoveEE 
(EE 
)EE 
;EE 
}FF 	
}GG 
}HH ˛(
CC:\MemoryGame\Client\Client\Views\Controls\ReportUserDialog.xaml.cs
	namespace 	
Client
 
. 
Views 
. 
Controls 
{ 
public 

partial 
class 
ReportUserDialog )
:* +
Window, 2
{ 
private 
readonly 
string 
_targetUsername  /
;/ 0
private 
readonly 
int 
_matchId %
;% &
public 
ReportUserDialog 
(  
string  &
targetUsername' 5
,5 6
int7 :
matchId; B
)B C
{ 	
InitializeComponent 
(  
)  !
;! "
_targetUsername 
= 
targetUsername ,
;, -
_matchId 
= 
matchId 
; 
TextTargetUser 
. 
Text 
=  !
$"" $
$str$ ,
{, -
_targetUsername- <
}< =
"= >
;> ?
} 	
private 
async 
void 
ButtonReport_Click -
(- .
object. 4
sender5 ;
,; <
RoutedEventArgs= L
eM N
)N O
{ 	
if 
( 
_targetUsername 
==  "
UserSession# .
.. /
Username/ 7
)7 8
{ 
new 
CustomMessageBox $
($ %
Lang 
. 
Global_Title_Error +
,+ ,
Lang- 1
.1 2-
!ReportUserDialog_Error_AutoReport2 S
,S T
this   
,   
MessageBoxType   (
.  ( )
Error  ) .
)  . /
.  / 0

ShowDialog  0 :
(  : ;
)  ; <
;  < =
this!! 
.!! 
Close!! 
(!! 
)!! 
;!! 
return"" 
;"" 
}## 
ButtonReport%% 
.%% 
	IsEnabled%% "
=%%# $
false%%% *
;%%* +
var&& 
client&& 
=&& 
UserServiceManager&& +
.&&+ ,
Instance&&, 4
.&&4 5
Client&&5 ;
;&&; <
try(( 
{)) 
var** 
response** 
=** 
await** $
client**% +
.**+ ,
ReportUserAsync**, ;
(**; <
UserSession**< G
.**G H
SessionToken**H T
,**T U
_targetUsername**V e
,**e f
_matchId**g o
)**o p
;**p q
if,, 
(,, 
response,, 
.,, 
Success,, $
),,$ %
{-- 
new.. 
CustomMessageBox.. (
(..( )
Lang// 
.// 0
$ReportUserDialog_Title_ReportSuccess// A
,//A B
Lang//C G
.//G H+
ReportUserDialog_Message_Report//H g
,//g h
this00 
,00 
MessageBoxType00 ,
.00, -
Success00- 4
)004 5
.005 6

ShowDialog006 @
(00@ A
)00A B
;00B C
this11 
.11 
Close11 
(11 
)11  
;11  !
}22 
else33 
{44 
new55 
CustomMessageBox55 (
(55( )
Lang66 
.66 
Global_Title_Error66 /
,66/ 0
	GetString661 :
(66: ;
response66; C
.66C D

MessageKey66D N
)66N O
,66O P
this77 
,77 
MessageBoxType77 ,
.77, -
Error77- 2
)772 3
.773 4

ShowDialog774 >
(77> ?
)77? @
;77@ A
ButtonReport99  
.99  !
	IsEnabled99! *
=99+ ,
true99- 1
;991 2
}:: 
};; 
catch<< 
(<< 
	Exception<< 
ex<< 
)<<  
{== 
ExceptionManager>>  
.>>  !
Handle>>! '
(>>' (
ex>>( *
,>>* +
this>>, 0
,>>0 1
(>>2 3
)>>3 4
=>>>5 7
ButtonReport>>8 D
.>>D E
	IsEnabled>>E N
=>>O P
true>>Q U
)>>U V
;>>V W
}?? 
}@@ 	
privateBB 
voidBB 
ButtonCancel_ClickBB '
(BB' (
objectBB( .
senderBB/ 5
,BB5 6
RoutedEventArgsBB7 F
eBBG H
)BBH I
{CC 	
thisDD 
.DD 
CloseDD 
(DD 
)DD 
;DD 
}EE 	
privateGG 
voidGG &
Window_MouseLeftButtonDownGG /
(GG/ 0
objectGG0 6
senderGG7 =
,GG= > 
MouseButtonEventArgsGG? S
eGGT U
)GGU V
{HH 	
DragMoveII 
(II 
)II 
;II 
}JJ 	
}KK 
}LL Ê
?C:\MemoryGame\Client\Client\Views\Controls\MatchSummary.xaml.cs
	namespace 	
Client
 
. 
Views 
. 
Controls 
{ 
public 

partial 
class 
MatchSummary %
:& '
Window( .
{ 
public 
MatchSummary 
( 
string "

winnerName# -
,- .
string/ 5
	scoreText6 ?
)? @
{ 	
InitializeComponent 
(  
)  !
;! "
var 
textMessageBrush  
=! "
(# $
SolidColorBrush$ 3
)3 4
Application4 ?
.? @
Current@ G
.G H
FindResourceH T
(T U
$strU l
)l m
;m n
if 
( 

winnerName 
== 
UserSession )
.) *
Username* 2
)2 3
{ 
TextBlockWinner 
.  
Text  $
=% &
Lang' +
.+ ,"
MatchSummary_Label_Win, B
;B C
} 
else 
if 
( 

winnerName 
==  "
Lang# '
.' ('
Singleplayer_Title_TimeOver( C
)C D
{ 
TextBlockWinner 
.  
Text  $
=% &

winnerName' 1
;1 2
TextBlockWinner 
.  

Foreground  *
=+ ,
textMessageBrush- =
;= >
} 
else 
{ 
TextBlockWinner 
.  
Text  $
=% &
$"' )
{) *

winnerName* 4
}4 5
$str5 6
{6 7
Lang7 ;
.; <#
MatchSummary_Label_Lost< S
}S T
"T U
;U V
TextBlockWinner 
.  

Foreground  *
=+ ,
textMessageBrush- =
;= >
}   
TextBlockScore"" 
."" 
Text"" 
=""  !
	scoreText""" +
;""+ ,
}## 	
private%% 
void%% 
ButtonAccept_Click%% '
(%%' (
object%%( .
sender%%/ 5
,%%5 6
RoutedEventArgs%%7 F
e%%G H
)%%H I
{&& 	
this'' 
.'' 
DialogResult'' 
='' 
true''  $
;''$ %
this(( 
.(( 
Close(( 
((( 
)(( 
;(( 
})) 	
private++ 
void++ &
Window_MouseLeftButtonDown++ /
(++/ 0
object++0 6
sender++7 =
,++= > 
MouseButtonEventArgs++? S
e++T U
)++U V
{,, 	
DragMove-- 
(-- 
)-- 
;-- 
}.. 	
}// 
}00 ù,
IC:\MemoryGame\Client\Client\Views\Controls\ConfirmationMessageBox.xaml.cs
	namespace 	
Client
 
. 
Views 
. 
Controls 
{ 
public

 

partial

 
class

 "
ConfirmationMessageBox

 /
:

0 1
Window

2 8
{ 
public 
enum 
ConfirmationBoxType '
{ 	
Information 
, 
Warning 
, 
Critic 
, 
Question 
} 	
public "
ConfirmationMessageBox %
(% &
string& ,
title- 2
,2 3
string4 :
message; B
,B C
WindowD J
ownerK P
,P Q
ConfirmationBoxTypeR e
typef j
)j k
{ 	
InitializeComponent 
(  
)  !
;! "
if 
( 
owner 
!= 
null 
&&  
owner! &
.& '
	IsVisible' 0
)0 1
{ 
this 
. 
Owner 
= 
owner "
;" #
} 
else 
{ 
this 
. !
WindowStartupLocation *
=+ ,!
WindowStartupLocation- B
.B C
CenterScreenC O
;O P
} 
TextBlockTitle   
.   
Text   
=    !
title  " '
;  ' (
TextBlockMessage!! 
.!! 
Text!! !
=!!" #
message!!$ +
;!!+ ,
SetStyle## 
(## 
type## 
)## 
;## 
}$$ 	
private&& 
void&& 
SetStyle&& 
(&& 
ConfirmationBoxType&& 1
type&&2 6
)&&6 7
{'' 	
SolidColorBrush(( 
borderBrush(( '
;((' (
SolidColorBrush)) 
textMessageBrush)) ,
=))- .
())/ 0
SolidColorBrush))0 ?
)))? @
Application))@ K
.))K L
Current))L S
.))S T
FindResource))T `
())` a
$str))a x
)))x y
;))y z
switch++ 
(++ 
type++ 
)++ 
{,, 
case-- 
ConfirmationBoxType-- (
.--( )
Warning--) 0
:--0 1
borderBrush.. 
=..  !
(.." #
SolidColorBrush..# 2
)..2 3
Application..3 >
...> ?
Current..? F
...F G
FindResource..G S
(..S T
$str..T f
)..f g
;..g h
break// 
;// 
case00 
ConfirmationBoxType00 (
.00( )
Information00) 4
:004 5
borderBrush11 
=11  !
(11" #
SolidColorBrush11# 2
)112 3
Application113 >
.11> ?
Current11? F
.11F G
FindResource11G S
(11S T
$str11T f
)11f g
;11g h
break22 
;22 
case33 
ConfirmationBoxType33 (
.33( )
Critic33) /
:33/ 0
borderBrush44 
=44  !
(44" #
SolidColorBrush44# 2
)442 3
Application443 >
.44> ?
Current44? F
.44F G
FindResource44G S
(44S T
$str44T _
)44_ `
;44` a
break55 
;55 
case66 
ConfirmationBoxType66 (
.66( )
Question66) 1
:661 2
borderBrush77 
=77  !
(77" #
SolidColorBrush77# 2
)772 3
Application773 >
.77> ?
Current77? F
.77F G
FindResource77G S
(77S T
$str77T a
)77a b
;77b c
break88 
;88 
default99 
:99 
borderBrush:: 
=::  !
(::" #
SolidColorBrush::# 2
)::2 3
Application::3 >
.::> ?
Current::? F
.::F G
FindResource::G S
(::S T
$str::T a
)::a b
;::b c
break;; 
;;; 
}<< 
MessageBorder== 
.== 

Background== $
===% &
borderBrush==' 2
;==2 3
TextBlockMessage>> 
.>> 

Foreground>> '
=>>( )
textMessageBrush>>* :
;>>: ;
}?? 	
privateAA 
voidAA 
ButtonAccept_ClickAA '
(AA' (
objectAA( .
senderAA/ 5
,AA5 6
RoutedEventArgsAA7 F
eAAG H
)AAH I
{BB 	
thisCC 
.CC 
DialogResultCC 
=CC 
trueCC  $
;CC$ %
}DD 	
privateFF 
voidFF 
ButtonCancel_ClickFF '
(FF' (
objectFF( .
senderFF/ 5
,FF5 6
RoutedEventArgsFF7 F
eFFG H
)FFH I
{GG 	
thisHH 
.HH 
DialogResultHH 
=HH 
falseHH  %
;HH% &
}II 	
privateKK 
voidKK &
Window_MouseLeftButtonDownKK /
(KK/ 0
objectKK0 6
senderKK7 =
,KK= > 
MouseButtonEventArgsKK? S
eKKT U
)KKU V
{LL 	
DragMoveMM 
(MM 
)MM 
;MM 
}NN 	
}OO 
}PP ¡*
CC:\MemoryGame\Client\Client\Views\Controls\CustomMessageBox.xaml.cs
	namespace 	
Client
 
. 
Views 
. 
Controls 
{ 
public 

partial 
class 
CustomMessageBox )
:* +
Window, 2
{ 
public 
enum 
MessageBoxType "
{ 	
Information 
, 
Success 
, 
Warning 
, 
Error 
} 	
public 
CustomMessageBox 
(  
String  &
title' ,
,, -
string. 4
message5 <
,< =
Window> D
ownerE J
,J K
MessageBoxTypeL Z
type[ _
)_ `
{ 	
InitializeComponent 
(  
)  !
;! "
if!! 
(!! 
owner!! 
!=!! 
null!! 
&&!!  
owner!!! &
.!!& '
	IsVisible!!' 0
)!!0 1
{"" 
this## 
.## 
Owner## 
=## 
owner## "
;##" #
}$$ 
else%% 
{&& 
this'' 
.'' !
WindowStartupLocation'' *
=''+ ,!
WindowStartupLocation''- B
.''B C
CenterScreen''C O
;''O P
}(( 
TextBlockTitle** 
.** 
Text** 
=**  !
title**" '
;**' (
TextBlockMessage++ 
.++ 
Text++ !
=++" #
message++$ +
;+++ ,
SetStyle-- 
(-- 
type-- 
)-- 
;-- 
}.. 	
private00 
void00 
SetStyle00 
(00 
MessageBoxType00 ,
type00- 1
)001 2
{11 	
SolidColorBrush22 
borderBrush22 '
;22' (
SolidColorBrush33 
textMessageBrush33 ,
=33- .
(33/ 0
SolidColorBrush330 ?
)33? @
Application33@ K
.33K L
Current33L S
.33S T
FindResource33T `
(33` a
$str33a x
)33x y
??33z |
Brushes	33} Ñ
.
33Ñ Ö
Black
33Ö ä
;
33ä ã
switch55 
(55 
type55 
)55 
{66 
case77 
MessageBoxType77 #
.77# $
Success77$ +
:77+ ,
borderBrush88 
=88  !
(88" #
SolidColorBrush88# 2
)882 3
Application883 >
.88> ?
Current88? F
.88F G
FindResource88G S
(88S T
$str88T _
)88_ `
;88` a
break99 
;99 
case:: 
MessageBoxType:: #
.::# $
Warning::$ +
:::+ ,
borderBrush;; 
=;;  !
(;;" #
SolidColorBrush;;# 2
);;2 3
Application;;3 >
.;;> ?
Current;;? F
.;;F G
FindResource;;G S
(;;S T
$str;;T f
);;f g
;;;g h
break<< 
;<< 
case== 
MessageBoxType== #
.==# $
Error==$ )
:==) *
borderBrush>> 
=>>  !
(>>" #
SolidColorBrush>># 2
)>>2 3
Application>>3 >
.>>> ?
Current>>? F
.>>F G
FindResource>>G S
(>>S T
$str>>T _
)>>_ `
;>>` a
break?? 
;?? 
case@@ 
MessageBoxType@@ #
.@@# $
Information@@$ /
:@@/ 0
borderBrushAA 
=AA  !
(AA" #
SolidColorBrushAA# 2
)AA2 3
ApplicationAA3 >
.AA> ?
CurrentAA? F
.AAF G
FindResourceAAG S
(AAS T
$strAAT f
)AAf g
;AAg h
breakBB 
;BB 
defaultCC 
:CC 
borderBrushDD 
=DD  !
(DD" #
SolidColorBrushDD# 2
)DD2 3
ApplicationDD3 >
.DD> ?
CurrentDD? F
.DDF G
FindResourceDDG S
(DDS T
$strDDT a
)DDa b
;DDb c
breakEE 
;EE 
}FF 
MessageBorderHH 
.HH 

BackgroundHH $
=HH% &
borderBrushHH' 2
;HH2 3
TextBlockMessageII 
.II 

ForegroundII '
=II( )
textMessageBrushII* :
;II: ;
}JJ 	
privateLL 
voidLL 
ButtonAccept_ClickLL '
(LL' (
objectLL( .
senderLL/ 5
,LL5 6
RoutedEventArgsLL7 F
eLLG H
)LLH I
{MM 	
thisNN 
.NN 
DialogResultNN 
=NN 
trueNN  $
;NN$ %
thisOO 
.OO 
CloseOO 
(OO 
)OO 
;OO 
}PP 	
privateRR 
voidRR &
Window_MouseLeftButtonDownRR /
(RR/ 0
objectRR0 6
senderRR7 =
,RR= > 
MouseButtonEventArgsRR? S
eRRT U
)RRU V
{SS 	
DragMoveTT 
(TT 
)TT 
;TT 
}UU 	
}VV 
}WW ≈#
.C:\MemoryGame\Client\Client\ViewModels\Card.cs
	namespace		 	
Client		
 
.		 

ViewModels		 
{

 
public 

class 
Card 
: "
INotifyPropertyChanged .
{ 
public 
int 
Id 
{ 
get 
; 
set  
;  !
}" #
public 
int 
PairId 
{ 
get 
;  
set! $
;$ %
}& '
private 
string 
frontImageResource )
;) *
private 
readonly 
string 
_backImageResource  2
=3 4
$str5 q
;q r
private 
bool 
	isFlipped 
; 
private 
bool 
	isMatched 
; 
public 
Card 
( 
int 
id 
, 
int 
pairId  &
,& '
string( .

frontImage/ 9
)9 :
{ 	
Id 
= 
id 
; 
PairId 
= 
pairId 
; 
frontImageResource 
=  

frontImage! +
;+ ,
	IsFlipped 
= 
false 
; 
	IsMatched 
= 
false 
; 
} 	
public 
void 
SetFrontImage !
(! "
string" (
	imagePath) 2
)2 3
{ 	
frontImageResource 
=  
	imagePath! *
;* +
OnPropertyChanged   
(   
nameof   $
(  $ %
DisplayImage  % 1
)  1 2
)  2 3
;  3 4
}!! 	
public## 
bool## 
	IsFlipped## 
{$$ 	
get%% 
{%% 
return%% 
	isFlipped%% "
;%%" #
}%%$ %
set&& 
{'' 
if(( 
((( 
	isFlipped(( 
!=((  
value((! &
)((& '
{)) 
	isFlipped** 
=** 
value**  %
;**% &
OnPropertyChanged++ %
(++% &
)++& '
;++' (
OnPropertyChanged,, %
(,,% &
nameof,,& ,
(,,, -
DisplayImage,,- 9
),,9 :
),,: ;
;,,; <
}-- 
}.. 
}// 	
public11 
bool11 
	IsMatched11 
{22 	
get33 
{33 
return33 
	isMatched33 "
;33" #
}33$ %
set44 
{55 
if66 
(66 
	isMatched66 
!=66  
value66! &
)66& '
{77 
	isMatched88 
=88 
value88  %
;88% &
OnPropertyChanged99 %
(99% &
)99& '
;99' (
OnPropertyChanged:: %
(::% &
nameof::& ,
(::, -
	IsMatched::- 6
)::6 7
)::7 8
;::8 9
OnPropertyChanged;; %
(;;% &
nameof;;& ,
(;;, -
DisplayImage;;- 9
);;9 :
);;: ;
;;;; <
}<< 
}== 
}>> 	
public@@ 
string@@ 
DisplayImage@@ "
{AA 	
getBB 
{CC 
ifDD 
(DD 
	IsMatchedDD 
||DD  
	IsFlippedDD! *
)DD* +
{EE 
returnFF 
frontImageResourceFF -
;FF- .
}GG 
returnHH 
_backImageResourceHH )
;HH) *
}II 
}JJ 	
publicLL 
eventLL '
PropertyChangedEventHandlerLL 0
PropertyChangedLL1 @
;LL@ A
	protectedMM 
voidMM 
OnPropertyChangedMM (
(MM( )
[MM) *
CallerMemberNameMM* :
]MM: ;
stringMM< B
nameMMC G
=MMH I
nullMMJ N
)MMN O
{NN 	
PropertyChangedOO 
?OO 
.OO 
InvokeOO #
(OO# $
thisOO$ (
,OO( )
newOO* -$
PropertyChangedEventArgsOO. F
(OOF G
nameOOG K
)OOK L
)OOL M
;OOM N
}PP 	
}QQ 
}RR ≈
4C:\MemoryGame\Client\Client\Models\LanguageOption.cs
	namespace 	
Client
 
. 
	Utilities 
{ 
public		 

class		 
LanguageOption		 
{

 
public 
string 
DisplayCultureName (
{) *
get+ .
;. /
set0 3
;3 4
}5 6
public 
string 
CultureCode !
{" #
get$ '
;' (
set) ,
;, -
}. /
} 
} §4
7C:\MemoryGame\Client\Client\Helpers\ValidationHelper.cs
	namespace 	
Client
 
. 
Helpers 
{ 
public		 

static		 
class		 
ValidationHelper		 (
{

 
public 
static 
ValidationCode $
ValidateUsername% 5
(5 6
string6 <
username= E
)E F
{ 	
if 
( 
string 
. 
IsNullOrEmpty $
($ %
username% -
)- .
). /
{ 
return 
ValidationCode %
.% &
UsernameEmpty& 3
;3 4
} 
if 
( 
username 
. 
Length 
>  !
$num" $
)$ %
{ 
return 
ValidationCode %
.% &
UsernameTooLong& 5
;5 6
} 
if 
( 
! 
username 
. 
All 
( 
ch  
=>! #
char$ (
.( )
IsLetterOrDigit) 8
(8 9
ch9 ;
); <
||= ?
ch@ B
==C E
$charF I
||J L
chM O
==P R
$charS V
)V W
)W X
{ 
return 
ValidationCode %
.% & 
UsernameInvalidChars& :
;: ;
} 
return 
ValidationCode !
.! "
Success" )
;) *
} 	
public 
static 
ValidationCode $
ValidatePassword% 5
(5 6
string6 <
password= E
)E F
{   	
if!! 
(!! 
string!! 
.!! 
IsNullOrEmpty!! $
(!!$ %
password!!% -
)!!- .
)!!. /
{"" 
return## 
ValidationCode## %
.##% &
PasswordEmpty##& 3
;##3 4
}$$ 
if&& 
(&& 
password&& 
.&& 
Length&& 
<&&  !
$num&&" #
||&&$ &
password&&' /
.&&/ 0
Length&&0 6
>&&7 8
$num&&9 ;
)&&; <
{'' 
return(( 
ValidationCode(( %
.((% &!
PasswordLengthInvalid((& ;
;((; <
})) 
if++ 
(++ 
!++ 
password++ 
.++ 
Any++ 
(++ 
char++ "
.++" #
IsUpper++# *
)++* +
)+++ ,
{,, 
return-- 
ValidationCode-- %
.--% & 
PasswordMissingUpper--& :
;--: ;
}.. 
if11 
(11 
!11 
password11 
.11 
Any11 
(11 
char11 "
.11" #
IsLower11# *
)11* +
)11+ ,
{22 
return33 
ValidationCode33 %
.33% & 
PasswordMissingLower33& :
;33: ;
}44 
if66 
(66 
!66 
password66 
.66 
Any66 
(66 
char66 "
.66" #
IsDigit66# *
)66* +
)66+ ,
{77 
return88 
ValidationCode88 %
.88% & 
PasswordMissingDigit88& :
;88: ;
}99 
return;; 
ValidationCode;; !
.;;! "
Success;;" )
;;;) *
}<< 	
public>> 
static>> 
ValidationCode>> $
ValidateEmail>>% 2
(>>2 3
string>>3 9
email>>: ?
)>>? @
{?? 	
if@@ 
(@@ 
string@@ 
.@@ 
IsNullOrEmpty@@ $
(@@$ %
email@@% *
)@@* +
)@@+ ,
{AA 
returnBB 
ValidationCodeBB %
.BB% &

EmailEmptyBB& 0
;BB0 1
}CC 
ifEE 
(EE 
!EE 
emailEE 
.EE 
ContainsEE 
(EE  
$strEE  #
)EE# $
||EE% '
!EE( )
emailEE) .
.EE. /
ContainsEE/ 7
(EE7 8
$strEE8 ;
)EE; <
)EE< =
{FF 
returnGG 
ValidationCodeGG %
.GG% &
EmailInvalidFormatGG& 8
;GG8 9
}HH 
returnJJ 
ValidationCodeJJ !
.JJ! "
SuccessJJ" )
;JJ) *
}KK 	
publicMM 
staticMM 
ValidationCodeMM $
ValidateVerifyCodeMM% 7
(MM7 8
stringMM8 >
codeMM? C
,MMC D
intMME H
requiredLengthMMI W
)MMW X
{NN 	
ifOO 
(OO 
stringOO 
.OO 
IsNullOrEmptyOO $
(OO$ %
codeOO% )
)OO) *
)OO* +
{PP 
returnQQ 
ValidationCodeQQ %
.QQ% &
	CodeEmptyQQ& /
;QQ/ 0
}RR 
ifSS 
(SS 
codeSS 
.SS 
LengthSS 
!=SS 
requiredLengthSS -
)SS- .
{TT 
returnUU 
ValidationCodeUU %
.UU% &
CodeLengthInvalidUU& 7
;UU7 8
}VV 
ifWW 
(WW 
!WW 
codeWW 
.WW 
AllWW 
(WW 
charWW 
.WW 
IsDigitWW &
)WW& '
)WW' (
{XX 
returnYY 
ValidationCodeYY %
.YY% &
CodeNotNumericYY& 4
;YY4 5
}ZZ 
return[[ 
ValidationCode[[ !
.[[! "
Success[[" )
;[[) *
}\\ 	
public^^ 
enum^^ 
ValidationCode^^ "
{__ 	
Success`` 
,`` 
UsernameEmptybb 
,bb 
UsernameTooLongcc 
,cc  
UsernameInvalidCharsdd  
,dd  !
PasswordEmptyff 
,ff !
PasswordLengthInvalidgg !
,gg! " 
PasswordMissingUpperhh  
,hh  ! 
PasswordMissingLowerii  
,ii  ! 
PasswordMissingDigitjj  
,jj  !

EmailEmptyll 
,ll 
EmailInvalidFormatmm 
,mm 
	CodeEmptyoo 
,oo 
CodeLengthInvalidpp 
,pp 
CodeNotNumericqq 
}ss 	
}tt 
}uu …á
9C:\MemoryGame\Client\Client\Helpers\LocalizationHelper.cs
	namespace 	
Client
 
. 
Helpers 
{ 
public		 

static		 
class		 
LocalizationHelper		 *
{

 
public 
static 
class 

ServerKeys &
{ 	
public 
const 
string 
InvalidCredentials  2
=3 4
$str5 V
;V W
public 
const 
string 
UserAlreadyLoggedIn  3
=4 5
$str6 X
;X Y
public 
const 
string 
AccountPenalized  0
=1 2
$str3 R
;R S
public 
const 
string 
SessionExpired  .
=/ 0
$str1 N
;N O
public 
const 
string 
InvalidToken  ,
=- .
$str/ J
;J K
public 
const 
string 
Unauthorized  ,
=- .
$str/ J
;J K
public 
const 
string 
PasswordInvalid  /
=0 1
$str2 P
;P Q
public 
const 
string 

EmailInUse  *
=+ ,
$str- F
;F G
public 
const 
string 
EmailSendFailed  /
=0 1
$str2 P
;P Q
public 
const 
string  
RegistrationNotFound  4
=5 6
$str7 Z
;Z [
public 
const 
string 
CodeInvalid  +
=, -
$str. H
;H I
public 
const 
string 
CodeExpired  +
=, -
$str. H
;H I
public 
const 
string 
InvalidUsername  /
=0 1
$str2 P
;P Q
public 
const 
string  
UsernameInvalidChars  4
=5 6
$str7 _
;_ `
public 
const 
string 
UsernameInUse  -
=. /
$str0 L
;L M
public   
const   
string   
UserNotFound    ,
=  - .
$str  / J
;  J K
public!! 
const!! 
string!! 
AlreadyRegistered!!  1
=!!2 3
$str!!4 T
;!!T U
public%% 
const%% 
string%% 
ImageInvalid%%  ,
=%%- .
$str%%/ J
;%%J K
public&& 
const&& 
string&& 
PasswordNull&&  ,
=&&- .
$str&&/ J
;&&J K
public'' 
const'' 
string''  
PasswordUpdateFailed''  4
=''5 6
$str''7 T
;''T U
public(( 
const(( 
string(( 
NewUsernameNull((  /
=((0 1
$str((2 R
;((R S
public)) 
const)) 
string)) 
SameUsername))  ,
=))- .
$str))/ J
;))J K
public** 
const** 
string** 
SocialDuplicate**  /
=**0 1
$str**2 Q
;**Q R
public++ 
const++ 
string++ 
NotFound++  (
=++) *
$str+++ B
;++B C
public// 
const// 
string// 
SelfAddFriend//  -
=//. /
$str//0 F
;//F G
public00 
const00 
string00 
AlreadyFriends00  .
=00/ 0
$str001 N
;00N O
public11 
const11 
string11 
RequestExists11  -
=11. /
$str110 L
;11L M
public22 
const22 
string22 
RequestNotFound22  /
=220 1
$str222 P
;22P Q
public33 
const33 
string33 

NotFriends33  *
=33+ ,
$str33- F
;33F G
public77 
const77 
string77  
ServiceErrorDatabase77  4
=775 6
$str777 U
;77U V
public88 
const88 
string88 
ServiceErrorUnknown88  3
=884 5
$str886 S
;88S T
public99 
const99 
string99 
ErrorDatabase99  -
=99. /
$str990 G
;99G H
public:: 
const:: 
string:: 
ErrorUnknown::  ,
=::- .
$str::/ E
;::E F
public;; 
const;; 
string;; 
ErrorDatabaseError;;  2
=;;3 4
$str;;5 Q
;;;Q R
public<< 
const<< 
string<<  
ErrorUnexpectedError<<  4
=<<5 6
$str<<7 U
;<<U V
public== 
const== 
string== 
ErrorUnknownError==  1
===2 3
$str==4 O
;==O P
}?? 	
publicAA 
staticAA 
stringAA 
	GetStringAA &
(AA& '
stringAA' -
serverMessageKeyAA. >
)AA> ?
{BB 	
ifCC 
(CC 
stringCC 
.CC 
IsNullOrEmptyCC $
(CC$ %
serverMessageKeyCC% 5
)CC5 6
)CC6 7
returnDD 
LangDD 
.DD '
Global_ServiceError_UnknownDD 7
;DD7 8
switchFF 
(FF 
serverMessageKeyFF $
)FF$ %
{GG 
caseII 

ServerKeysII 
.II  
InvalidCredentialsII  2
:II2 3
returnJJ 
LangJJ 
.JJ  +
Global_Error_InvalidCredentialsJJ  ?
;JJ? @
caseLL 

ServerKeysLL 
.LL  
UserAlreadyLoggedInLL  3
:LL3 4
returnMM 
LangMM 
.MM  +
Global_Error_InvalidCredentialsMM  ?
;MM? @
caseOO 

ServerKeysOO 
.OO  
SessionExpiredOO  .
:OO. /
casePP 

ServerKeysPP 
.PP  
InvalidTokenPP  ,
:PP, -
returnQQ 
LangQQ 
.QQ  %
Global_Error_InvalidTokenQQ  9
;QQ9 :
caseSS 

ServerKeysSS 
.SS  
AccountPenalizedSS  0
:SS0 1
returnTT 
$strTT =
;TT= >
caseWW 

ServerKeysWW 
.WW  
PasswordInvalidWW  /
:WW/ 0
returnXX 
LangXX 
.XX  (
Global_Error_PasswordInvalidXX  <
;XX< =
caseYY 

ServerKeysYY 
.YY  

EmailInUseYY  *
:YY* +
returnZZ 
LangZZ 
.ZZ  #
Global_Error_EmailInUseZZ  7
;ZZ7 8
case[[ 

ServerKeys[[ 
.[[  
EmailSendFailed[[  /
:[[/ 0
return\\ 
Lang\\ 
.\\  (
Global_Error_EmailSendFailed\\  <
;\\< =
case]] 

ServerKeys]] 
.]]   
RegistrationNotFound]]  4
:]]4 5
return^^ 
Lang^^ 
.^^  -
!Global_Error_RegistrationNotFound^^  A
;^^A B
case__ 

ServerKeys__ 
.__  
CodeInvalid__  +
:__+ ,
return`` 
Lang`` 
.``  $
Global_Error_CodeInvalid``  8
;``8 9
caseaa 

ServerKeysaa 
.aa  
CodeExpiredaa  +
:aa+ ,
returnbb 
Langbb 
.bb  $
Global_Error_CodeExpiredbb  8
;bb8 9
casecc 

ServerKeyscc 
.cc  
InvalidUsernamecc  /
:cc/ 0
returndd 
Langdd 
.dd  (
Global_Error_InvalidUsernamedd  <
;dd< =
caseee 

ServerKeysee 
.ee  
UsernameInUseee  -
:ee- .
returnff 
Langff 
.ff  &
Global_Error_UsernameInUseff  :
;ff: ;
caseii 

ServerKeysii 
.ii  
UserNotFoundii  ,
:ii, -
returnjj 
Langjj 
.jj  %
Global_Error_UserNotFoundjj  9
;jj9 :
casekk 

ServerKeyskk 
.kk  
ImageInvalidkk  ,
:kk, -
returnll 
Langll 
.ll  %
Global_Error_ImageInvalidll  9
;ll9 :
casemm 

ServerKeysmm 
.mm   
PasswordUpdateFailedmm  4
:mm4 5
returnnn 
Langnn 
.nn  1
%EditProfile_Label_ErrorPasswordUpdatenn  E
;nnE F
caseoo 

ServerKeysoo 
.oo  
SameUsernameoo  ,
:oo, -
returnpp 
Langpp 
.pp  /
#EditProfile_Label_ErrorSameUsernamepp  C
;ppC D
casess 

ServerKeysss 
.ss  
SelfAddFriendss  -
:ss- .
returntt 
Langtt 
.tt   
Social_Error_SelfAddtt  4
;tt4 5
caseuu 

ServerKeysuu 
.uu  
AlreadyFriendsuu  .
:uu. /
returnvv 
Langvv 
.vv  '
Social_Error_AlreadyFriendsvv  ;
;vv; <
caseww 

ServerKeysww 
.ww  
RequestExistsww  -
:ww- .
returnxx 
Langxx 
.xx  &
Social_Error_RequestExistsxx  :
;xx: ;
case{{ 

ServerKeys{{ 
.{{   
ServiceErrorDatabase{{  4
:{{4 5
case|| 
$str|| ,
:||, -
case}} 
$str}} 1
:}}1 2
return~~ 
Lang~~ 
.~~  '
Global_ServiceError_Unknown~~  ;
;~~; <
case
ÄÄ 

ServerKeys
ÄÄ 
.
ÄÄ  !
ServiceErrorUnknown
ÄÄ  3
:
ÄÄ3 4
default
ÅÅ 
:
ÅÅ 
return
ÇÇ 
Lang
ÇÇ 
.
ÇÇ  )
Global_ServiceError_Unknown
ÇÇ  ;
;
ÇÇ; <
}
ÉÉ 
}
ÑÑ 	
public
ÜÜ 
static
ÜÜ 
string
ÜÜ 
	GetString
ÜÜ &
(
ÜÜ& '
ValidationHelper
ÜÜ' 7
.
ÜÜ7 8
ValidationCode
ÜÜ8 F
code
ÜÜG K
)
ÜÜK L
{
áá 	
switch
àà 
(
àà 
code
àà 
)
àà 
{
ââ 
case
ää 
ValidationHelper
ää %
.
ää% &
ValidationCode
ää& 4
.
ää4 5
Success
ää5 <
:
ää< =
return
ãã 
string
ãã !
.
ãã! "
Empty
ãã" '
;
ãã' (
case
éé 
ValidationHelper
éé %
.
éé% &
ValidationCode
éé& 4
.
éé4 5

EmailEmpty
éé5 ?
:
éé? @
return
èè 
Lang
èè 
.
èè  *
Global_ValidationEmail_Empty
èè  <
;
èè< =
case
êê 
ValidationHelper
êê %
.
êê% &
ValidationCode
êê& 4
.
êê4 5 
EmailInvalidFormat
êê5 G
:
êêG H
return
ëë 
Lang
ëë 
.
ëë  2
$Global_ValidationEmail_InvalidFormat
ëë  D
;
ëëD E
case
îî 
ValidationHelper
îî %
.
îî% &
ValidationCode
îî& 4
.
îî4 5
PasswordEmpty
îî5 B
:
îîB C
return
ïï 
Lang
ïï 
.
ïï  -
Global_ValidationPassword_Empty
ïï  ?
;
ïï? @
case
óó 
ValidationHelper
óó %
.
óó% &
ValidationCode
óó& 4
.
óó4 5#
PasswordLengthInvalid
óó5 J
:
óóJ K
return
òò 
Lang
òò 
.
òò  5
'Global_ValidationPassword_LengthInvalid
òò  G
;
òòG H
case
öö 
ValidationHelper
öö %
.
öö% &
ValidationCode
öö& 4
.
öö4 5"
PasswordMissingUpper
öö5 I
:
ööI J
return
õõ 
Lang
õõ 
.
õõ  4
&Global_ValidationPassword_MissingUpper
õõ  F
;
õõF G
case
ùù 
ValidationHelper
ùù %
.
ùù% &
ValidationCode
ùù& 4
.
ùù4 5"
PasswordMissingLower
ùù5 I
:
ùùI J
return
ûû 
Lang
ûû 
.
ûû  4
&Global_ValidationPassword_MissingLower
ûû  F
;
ûûF G
case
†† 
ValidationHelper
†† %
.
††% &
ValidationCode
††& 4
.
††4 5"
PasswordMissingDigit
††5 I
:
††I J
return
°° 
Lang
°° 
.
°°  4
&Global_ValidationPassword_MissingDigit
°°  F
;
°°F G
case
§§ 
ValidationHelper
§§ %
.
§§% &
ValidationCode
§§& 4
.
§§4 5
UsernameEmpty
§§5 B
:
§§B C
return
•• 
Lang
•• 
.
••  -
Global_ValidationUsername_Empty
••  ?
;
••? @
case
ßß 
ValidationHelper
ßß %
.
ßß% &
ValidationCode
ßß& 4
.
ßß4 5
UsernameTooLong
ßß5 D
:
ßßD E
return
®® 
Lang
®® 
.
®®  /
!Global_ValidationUsername_TooLong
®®  A
;
®®A B
case
™™ 
ValidationHelper
™™ %
.
™™% &
ValidationCode
™™& 4
.
™™4 5"
UsernameInvalidChars
™™5 I
:
™™I J
return
´´ 
Lang
´´ 
.
´´  4
&Global_ValidationUsername_InvalidChars
´´  F
;
´´F G
case
ÆÆ 
ValidationHelper
ÆÆ %
.
ÆÆ% &
ValidationCode
ÆÆ& 4
.
ÆÆ4 5
	CodeEmpty
ÆÆ5 >
:
ÆÆ> ?
return
ØØ 
Lang
ØØ 
.
ØØ  )
Global_ValidationCode_Empty
ØØ  ;
;
ØØ; <
case
±± 
ValidationHelper
±± %
.
±±% &
ValidationCode
±±& 4
.
±±4 5
CodeLengthInvalid
±±5 F
:
±±F G
return
≤≤ 
Lang
≤≤ 
.
≤≤  1
#Global_ValidationCode_LengthInvalid
≤≤  C
;
≤≤C D
case
¥¥ 
ValidationHelper
¥¥ %
.
¥¥% &
ValidationCode
¥¥& 4
.
¥¥4 5
CodeNotNumeric
¥¥5 C
:
¥¥C D
return
µµ 
Lang
µµ 
.
µµ  .
 Global_ValidationCode_NotNumeric
µµ  @
;
µµ@ A
default
∑∑ 
:
∑∑ 
return
∏∏ 
Lang
∏∏ 
.
∏∏  )
Global_ServiceError_Unknown
∏∏  ;
;
∏∏; <
}
ππ 
}
∫∫ 	
public
ºº 
static
ºº 
string
ºº 
	GetString
ºº &
(
ºº& '
	Exception
ºº' 0
ex
ºº1 3
)
ºº3 4
{
ΩΩ 	
if
ææ 
(
ææ 
ex
ææ 
is
ææ '
EndpointNotFoundException
ææ /
)
ææ/ 0
{
øø 
return
¿¿ 
Lang
¿¿ 
.
¿¿ )
Global_ServiceError_Offline
¿¿ 7
;
¿¿7 8
}
¡¡ 
if
¬¬ 
(
¬¬ 
ex
¬¬ 
is
¬¬ $
CommunicationException
¬¬ ,
)
¬¬, -
{
√√ 
return
ƒƒ 
Lang
ƒƒ 
.
ƒƒ -
Global_ServiceError_NetworkDown
ƒƒ ;
;
ƒƒ; <
}
≈≈ 
if
∆∆ 
(
∆∆ 
ex
∆∆ 
is
∆∆ 
TimeoutException
∆∆ &
)
∆∆& '
{
«« 
return
»» 
Lang
»» 
.
»» -
Global_ServiceError_NetworkDown
»» ;
;
»»; <
}
…… 
return
   
Lang
   
.
   )
Global_ServiceError_Unknown
   3
;
  3 4
}
ÀÀ 	
public
ÕÕ 
static
ÕÕ 
void
ÕÕ 
ApplyLanguageFont
ÕÕ ,
(
ÕÕ, -
)
ÕÕ- .
{
ŒŒ 	
string
œœ 
fontName
œœ 
=
œœ 
Lang
œœ "
.
œœ" #
Global_FontName
œœ# 2
;
œœ2 3
string
–– 
fontPath
–– 
=
–– 
$"
––  
$str
––  G
{
––G H
fontName
––H P
}
––P Q
"
––Q R
;
––R S

FontFamily
—— 
newFont
—— 
=
——  
new
——! $

FontFamily
——% /
(
——/ 0
fontPath
——0 8
)
——8 9
;
——9 :
Application
”” 
.
”” 
Current
”” 
.
””  
	Resources
””  )
[
””) *
$str
””* 5
]
””5 6
=
””7 8
newFont
””9 @
;
””@ A
Application
‘‘ 
.
‘‘ 
Current
‘‘ 
.
‘‘  
	Resources
‘‘  )
[
‘‘) *
$str
‘‘* 4
]
‘‘4 5
=
‘‘6 7
newFont
‘‘8 ?
;
‘‘? @
}
’’ 	
}
÷÷ 
}◊◊ ˝
7C:\MemoryGame\Client\Client\Models\GameConfiguration.cs
	namespace 	
Client
 
. 
Models 
{ 
public		 

class		 
GameConfiguration		 "
{

 
public 
int 
NumberOfCards  
{! "
get# &
;& '
set( +
;+ ,
}- .
public 
int 
TimeLimitSeconds #
{$ %
get& )
;) *
set+ .
;. /
}0 1
public 
string 
DifficultyLevel %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
public 
int 

NumberRows 
{ 
get  #
;# $
set% (
;( )
}* +
public 
int 
NumberColumns  
{! "
get# &
;& '
set( +
;+ ,
}- .
public 
GameConfiguration  
(  !
)! "
{# $
}% &
public 
GameConfiguration  
(  !
int! $
cards% *
,* +
int, /
seconds0 7
,7 8
int9 <
rows= A
,A B
intC F
columnsG N
,N O
stringP V

difficultyW a
)a b
{ 	
NumberOfCards 
= 
cards !
;! "
TimeLimitSeconds 
= 
seconds &
;& '

NumberRows 
= 
rows 
; 
NumberColumns 
= 
columns #
;# $
DifficultyLevel 
= 

difficulty (
;( )
} 	
} 
} ëç
/C:\MemoryGame\Client\Client\Core\GameManager.cs
	namespace 	
Client
 
. 
Core 
{ 
public 

class 
GameManager 
{ 
public 
event 
Action 
< 
string "
>" #
TimerUpdated$ 0
;0 1
public 
event 
Action 
< 
int 
>  
ScoreUpdated! -
;- .
public 
event 
Action 
GameWon #
;# $
public 
event 
Action 
GameLost $
;$ %
public 
event 
Action 
TurnTimeEnded )
;) *
private   
DispatcherTimer   

_gameTimer    *
;  * +
private!! 
TimeSpan!! 
	_timeLeft!! "
;!!" #
private"" 
int"" 
_score"" 
;"" 
private## 
bool## 
_isProcessingTurn## &
;##& '
private$$ 
Card$$ 
_firstCardFlipped$$ &
;$$& '
private%% 
readonly%%  
ObservableCollection%% -
<%%- .
Card%%. 2
>%%2 3
_cardsOnBoard%%4 A
;%%A B
private&& 
int&&  
_turnDurationSeconds&& (
;&&( )
public** 
bool** 
IsMultiplayerMode** %
{**& '
get**( +
;**+ ,
set**- 0
;**0 1
}**2 3
=**4 5
false**6 ;
;**; <
public,, 
GameManager,, 
(,,  
ObservableCollection,, /
<,,/ 0
Card,,0 4
>,,4 5
cardsCollection,,6 E
),,E F
{-- 	
_cardsOnBoard.. 
=.. 
cardsCollection.. +
??.., .
throw../ 4
new..5 8!
ArgumentNullException..9 N
(..N O
nameof..O U
(..U V
cardsCollection..V e
)..e f
)..f g
;..g h
InitializeTimer// 
(// 
)// 
;// 
}00 	
private22 
void22 
InitializeTimer22 $
(22$ %
)22% &
{33 	

_gameTimer44 
=44 
new44 
DispatcherTimer44 ,
(44, -
)44- .
;44. /

_gameTimer55 
.55 
Interval55 
=55  !
TimeSpan55" *
.55* +
FromSeconds55+ 6
(556 7
$num557 8
)558 9
;559 :

_gameTimer66 
.66 
Tick66 
+=66 
GameTimer_Tick66 -
;66- .
}77 	
private99 
void99 
GameTimer_Tick99 #
(99# $
object99$ *
sender99+ 1
,991 2
	EventArgs993 <
e99= >
)99> ?
{:: 	
try;; 
{<< 
if== 
(== 
	_timeLeft== 
.== 
TotalSeconds== *
>==+ ,
$num==- .
)==. /
{>> 
	_timeLeft?? 
=?? 
	_timeLeft??  )
.??) *
Subtract??* 2
(??2 3
TimeSpan??3 ;
.??; <
FromSeconds??< G
(??G H
$num??H I
)??I J
)??J K
;??K L
TimerUpdated@@  
?@@  !
.@@! "
Invoke@@" (
(@@( )
	_timeLeft@@) 2
.@@2 3
ToString@@3 ;
(@@; <
$str@@< E
)@@E F
)@@F G
;@@G H
}AA 
elseBB 
{CC !
HandleTimerExpirationDD )
(DD) *
)DD* +
;DD+ ,
}EE 
}FF 
catchGG 
(GG 
	ExceptionGG 
exGG 
)GG  
{HH 
StopGameII 
(II 
)II 
;II 
SystemJJ 
.JJ 
DiagnosticsJJ "
.JJ" #
DebugJJ# (
.JJ( )
	WriteLineJJ) 2
(JJ2 3
$"JJ3 5
$strJJ5 Q
{JJQ R
exJJR T
.JJT U
MessageJJU \
}JJ\ ]
"JJ] ^
)JJ^ _
;JJ_ `
}KK 
}LL 	
privateNN 
voidNN !
HandleTimerExpirationNN *
(NN* +
)NN+ ,
{OO 	
ifPP 
(PP 
IsMultiplayerModePP !
)PP! "
{QQ 

_gameTimerRR 
.RR 
StopRR 
(RR  
)RR  !
;RR! "
TimerUpdatedSS 
?SS 
.SS 
InvokeSS $
(SS$ %
$strSS% ,
)SS, -
;SS- .
TurnTimeEndedTT 
?TT 
.TT 
InvokeTT %
(TT% &
)TT& '
;TT' (
}UU 
elseVV 
{WW 
StopGameXX 
(XX 
)XX 
;XX 
TimerUpdatedYY 
?YY 
.YY 
InvokeYY $
(YY$ %
$strYY% ,
)YY, -
;YY- .
GameLostZZ 
?ZZ 
.ZZ 
InvokeZZ  
(ZZ  !
)ZZ! "
;ZZ" #
}[[ 
}\\ 	
public^^ 
void^^ !
StartSingleplayerGame^^ )
(^^) *
GameConfiguration^^* ;
configuration^^< I
)^^I J
{__ 	
IsMultiplayerMode`` 
=`` 
false``  %
;``% &
ResetGameStateaa 
(aa 
configurationaa (
.aa( )
TimeLimitSecondsaa) 9
)aa9 :
;aa: ;
varcc 
deckcc 
=cc 
GenerateRandomDeckcc )
(cc) *
configurationcc* 7
.cc7 8
NumberOfCardscc8 E
)ccE F
;ccF G
foreachdd 
(dd 
vardd 
carddd 
indd  
deckdd! %
)dd% &
{ee 
_cardsOnBoardff 
.ff 
Addff !
(ff! "
cardff" &
)ff& '
;ff' (
}gg 

_gameTimerii 
.ii 
Startii 
(ii 
)ii 
;ii 
}jj 	
publicll 
voidll  
StartMultiplayerGamell (
(ll( )
GameConfigurationll) :
configurationll; H
,llH I
ListllJ N
<llN O
CardInfollO W
>llW X
serverCardsllY d
)lld e
{mm 	
IsMultiplayerModenn 
=nn 
truenn  $
;nn$ % 
_turnDurationSecondsoo  
=oo! "
configurationoo# 0
.oo0 1
TimeLimitSecondsoo1 A
;ooA B
ResetGameStateqq 
(qq  
_turnDurationSecondsqq /
)qq/ 0
;qq0 1
intss 
indexss 
=ss 
$numss 
;ss 
foreachtt 
(tt 
vartt 
infott 
intt  
serverCardstt! ,
)tt, -
{uu 
stringvv 
	imagePathvv  
=vv! "
$"vv# %
{vv% &
GameConstantsvv& 3
.vv3 4"
ColorCardFrontBasePathvv4 J
}vvJ K
{vvK L
infovvL P
.vvP Q
ImageIdentifiervvQ `
}vv` a
$strvva e
"vve f
;vvf g
varww 
newCardww 
=ww 
newww !
Cardww" &
(ww& '
indexww' ,
,ww, -
infoww. 2
.ww2 3
CardIdww3 9
,ww9 :
	imagePathww; D
)wwD E
;wwE F
newCardyy 
.yy 
	IsFlippedyy !
=yy" #
falseyy$ )
;yy) *
_cardsOnBoardzz 
.zz 
Addzz !
(zz! "
newCardzz" )
)zz) *
;zz* +
index{{ 
++{{ 
;{{ 
}|| 
}}} 	
public 
void 
UpdateTurnDuration &
(& '
int' *
seconds+ 2
)2 3
{
ÄÄ 	"
_turnDurationSeconds
ÅÅ  
=
ÅÅ! "
seconds
ÅÅ# *
;
ÅÅ* +
}
ÇÇ 	
public
ÑÑ 
void
ÑÑ 
ResetTurnTimer
ÑÑ "
(
ÑÑ" #
)
ÑÑ# $
{
ÖÖ 	
	_timeLeft
ÜÜ 
=
ÜÜ 
TimeSpan
ÜÜ  
.
ÜÜ  !
FromSeconds
ÜÜ! ,
(
ÜÜ, -"
_turnDurationSeconds
ÜÜ- A
)
ÜÜA B
;
ÜÜB C
TimerUpdated
áá 
?
áá 
.
áá 
Invoke
áá  
(
áá  !
	_timeLeft
áá! *
.
áá* +
ToString
áá+ 3
(
áá3 4
$str
áá4 =
)
áá= >
)
áá> ?
;
áá? @

_gameTimer
ââ 
.
ââ 
Stop
ââ 
(
ââ 
)
ââ 
;
ââ 

_gameTimer
ää 
.
ää 
Start
ää 
(
ää 
)
ää 
;
ää 
}
ãã 	
private
çç 
void
çç 
ResetGameState
çç #
(
çç# $
int
çç$ '
seconds
çç( /
)
çç/ 0
{
éé 	
	_timeLeft
èè 
=
èè 
TimeSpan
èè  
.
èè  !
FromSeconds
èè! ,
(
èè, -
seconds
èè- 4
)
èè4 5
;
èè5 6
_score
êê 
=
êê 
$num
êê 
;
êê 
_firstCardFlipped
ëë 
=
ëë 
null
ëë  $
;
ëë$ %
_isProcessingTurn
íí 
=
íí 
false
íí  %
;
íí% &
_cardsOnBoard
ìì 
.
ìì 
Clear
ìì 
(
ìì  
)
ìì  !
;
ìì! "
TimerUpdated
ïï 
?
ïï 
.
ïï 
Invoke
ïï  
(
ïï  !
	_timeLeft
ïï! *
.
ïï* +
ToString
ïï+ 3
(
ïï3 4
$str
ïï4 =
)
ïï= >
)
ïï> ?
;
ïï? @
ScoreUpdated
ññ 
?
ññ 
.
ññ 
Invoke
ññ  
(
ññ  !
$num
ññ! "
)
ññ" #
;
ññ# $
}
óó 	
private
ôô 
static
ôô 
List
ôô 
<
ôô 
Card
ôô  
>
ôô  ! 
GenerateRandomDeck
ôô" 4
(
ôô4 5
int
ôô5 8
numberOfCards
ôô9 F
)
ôôF G
{
öö 	
List
õõ 
<
õõ 
string
õõ 
>
õõ 

imagePaths
õõ #
=
õõ$ %
new
õõ& )
List
õõ* .
<
õõ. /
string
õõ/ 5
>
õõ5 6
{
úú 
$str
ùù 
,
ùù 
$str
ùù 
,
ùù  
$str
ùù! &
,
ùù& '
$str
ùù( 0
,
ùù0 1
$str
ùù2 9
,
ùù9 :
$str
ùù; @
,
ùù@ A
$str
ûû 
,
ûû 
$str
ûû 
,
ûû  
$str
ûû! (
,
ûû( )
$str
ûû* 0
,
ûû0 1
$str
ûû2 9
,
ûû9 :
$str
ûû; A
}
üü 
;
üü 
List
°° 
<
°° 
Card
°° 
>
°° 
deck
°° 
=
°° 
new
°° !
List
°°" &
<
°°& '
Card
°°' +
>
°°+ ,
(
°°, -
)
°°- .
;
°°. /
int
¢¢ 
pairsNeeded
¢¢ 
=
¢¢ 
numberOfCards
¢¢ +
/
¢¢, -
$num
¢¢. /
;
¢¢/ 0
for
§§ 
(
§§ 
int
§§ 
i
§§ 
=
§§ 
$num
§§ 
;
§§ 
i
§§ 
<
§§ 
pairsNeeded
§§  +
;
§§+ ,
i
§§- .
++
§§. 0
)
§§0 1
{
•• 
string
¶¶ 
imgName
¶¶ 
=
¶¶  

imagePaths
¶¶! +
[
¶¶+ ,
i
¶¶, -
%
¶¶. /

imagePaths
¶¶0 :
.
¶¶: ;
Count
¶¶; @
]
¶¶@ A
;
¶¶A B
string
ßß 
fullPath
ßß 
=
ßß  !
$"
ßß" $
{
ßß$ %
GameConstants
ßß% 2
.
ßß2 3$
ColorCardFrontBasePath
ßß3 I
}
ßßI J
{
ßßJ K
imgName
ßßK R
}
ßßR S
$str
ßßS W
"
ßßW X
;
ßßX Y
deck
©© 
.
©© 
Add
©© 
(
©© 
new
©© 
Card
©© !
(
©©! "
i
©©" #
*
©©$ %
$num
©©& '
,
©©' (
i
©©) *
,
©©* +
fullPath
©©, 4
)
©©4 5
)
©©5 6
;
©©6 7
deck
™™ 
.
™™ 
Add
™™ 
(
™™ 
new
™™ 
Card
™™ !
(
™™! "
i
™™" #
*
™™$ %
$num
™™& '
+
™™( )
$num
™™* +
,
™™+ ,
i
™™- .
,
™™. /
fullPath
™™0 8
)
™™8 9
)
™™9 :
;
™™: ;
}
´´ 
return
≠≠ 
deck
≠≠ 
.
≠≠ 
OrderBy
≠≠ 
(
≠≠  
x
≠≠  !
=>
≠≠" $
Guid
≠≠% )
.
≠≠) *
NewGuid
≠≠* 1
(
≠≠1 2
)
≠≠2 3
)
≠≠3 4
.
≠≠4 5
ToList
≠≠5 ;
(
≠≠; <
)
≠≠< =
;
≠≠= >
}
ÆÆ 	
public
¥¥ 
async
¥¥ 
Task
¥¥ 
HandleCardClick
¥¥ )
(
¥¥) *
Card
¥¥* .
clickedCard
¥¥/ :
)
¥¥: ;
{
µµ 	
if
∂∂ 
(
∂∂ 
IsMultiplayerMode
∂∂ !
)
∂∂! "
{
∑∑ 
return
∏∏ 
;
∏∏ 
}
ππ 
if
ªª 
(
ªª 
_isProcessingTurn
ªª !
||
ªª" $
clickedCard
ªª% 0
.
ªª0 1
	IsFlipped
ªª1 :
||
ªª; =
clickedCard
ªª> I
.
ªªI J
	IsMatched
ªªJ S
)
ªªS T
{
ºº 
return
ΩΩ 
;
ΩΩ 
}
ææ 
clickedCard
¿¿ 
.
¿¿ 
	IsFlipped
¿¿ !
=
¿¿" #
true
¿¿$ (
;
¿¿( )
if
¬¬ 
(
¬¬ 
_firstCardFlipped
¬¬ !
==
¬¬" $
null
¬¬% )
)
¬¬) *
{
√√ 
_firstCardFlipped
ƒƒ !
=
ƒƒ" #
clickedCard
ƒƒ$ /
;
ƒƒ/ 0
}
≈≈ 
else
∆∆ 
{
«« 
_isProcessingTurn
»» !
=
»»" #
true
»»$ (
;
»»( )
if
   
(
   
_firstCardFlipped
   %
.
  % &
PairId
  & ,
==
  - /
clickedCard
  0 ;
.
  ; <
PairId
  < B
)
  B C
{
ÀÀ 
await
ÃÃ 
ProcessMatch
ÃÃ &
(
ÃÃ& '
clickedCard
ÃÃ' 2
)
ÃÃ2 3
;
ÃÃ3 4
}
ÕÕ 
else
ŒŒ 
{
œœ 
await
–– 
ProcessMismatch
–– )
(
––) *
clickedCard
––* 5
)
––5 6
;
––6 7
}
—— 
_firstCardFlipped
”” !
=
””" #
null
””$ (
;
””( )
_isProcessingTurn
‘‘ !
=
‘‘" #
false
‘‘$ )
;
‘‘) *
}
’’ 
}
÷÷ 	
private
ÿÿ 
async
ÿÿ 
Task
ÿÿ 
ProcessMatch
ÿÿ '
(
ÿÿ' (
Card
ÿÿ( ,

secondCard
ÿÿ- 7
)
ÿÿ7 8
{
ŸŸ 	
await
⁄⁄ 
Task
⁄⁄ 
.
⁄⁄ 
Delay
⁄⁄ 
(
⁄⁄ 
GameConstants
⁄⁄ *
.
⁄⁄* + 
MatchFeedbackDelay
⁄⁄+ =
)
⁄⁄= >
;
⁄⁄> ?
_firstCardFlipped
‹‹ 
.
‹‹ 
	IsMatched
‹‹ '
=
‹‹( )
true
‹‹* .
;
‹‹. /

secondCard
›› 
.
›› 
	IsMatched
››  
=
››! "
true
››# '
;
››' (
_score
ﬂﬂ 
+=
ﬂﬂ 
GameConstants
ﬂﬂ #
.
ﬂﬂ# $
PointsPerMatch
ﬂﬂ$ 2
;
ﬂﬂ2 3
ScoreUpdated
‡‡ 
?
‡‡ 
.
‡‡ 
Invoke
‡‡  
(
‡‡  !
_score
‡‡! '
)
‡‡' (
;
‡‡( )
CheckWinCondition
‚‚ 
(
‚‚ 
)
‚‚ 
;
‚‚  
}
„„ 	
private
ÂÂ 
async
ÂÂ 
Task
ÂÂ 
ProcessMismatch
ÂÂ *
(
ÂÂ* +
Card
ÂÂ+ /

secondCard
ÂÂ0 :
)
ÂÂ: ;
{
ÊÊ 	
await
ÁÁ 
Task
ÁÁ 
.
ÁÁ 
Delay
ÁÁ 
(
ÁÁ 
GameConstants
ÁÁ *
.
ÁÁ* +#
MismatchFeedbackDelay
ÁÁ+ @
)
ÁÁ@ A
;
ÁÁA B
_firstCardFlipped
ÈÈ 
.
ÈÈ 
	IsFlipped
ÈÈ '
=
ÈÈ( )
false
ÈÈ* /
;
ÈÈ/ 0

secondCard
ÍÍ 
.
ÍÍ 
	IsFlipped
ÍÍ  
=
ÍÍ! "
false
ÍÍ# (
;
ÍÍ( )
}
ÎÎ 	
private
ÌÌ 
void
ÌÌ 
CheckWinCondition
ÌÌ &
(
ÌÌ& '
)
ÌÌ' (
{
ÓÓ 	
if
ÔÔ 
(
ÔÔ 
_cardsOnBoard
ÔÔ 
.
ÔÔ 
All
ÔÔ !
(
ÔÔ! "
c
ÔÔ" #
=>
ÔÔ$ &
c
ÔÔ' (
.
ÔÔ( )
	IsMatched
ÔÔ) 2
)
ÔÔ2 3
)
ÔÔ3 4
{
 
StopGame
ÒÒ 
(
ÒÒ 
)
ÒÒ 
;
ÒÒ 
GameWon
ÚÚ 
?
ÚÚ 
.
ÚÚ 
Invoke
ÚÚ 
(
ÚÚ  
)
ÚÚ  !
;
ÚÚ! "
}
ÛÛ 
}
ÙÙ 	
public
ˆˆ 
void
ˆˆ 
StopGame
ˆˆ 
(
ˆˆ 
)
ˆˆ 
{
˜˜ 	

_gameTimer
¯¯ 
?
¯¯ 
.
¯¯ 
Stop
¯¯ 
(
¯¯ 
)
¯¯ 
;
¯¯ 
}
˘˘ 	
}
˙˙ 
}˚˚ ˙G
6C:\MemoryGame\Client\Client\Core\GameServiceManager.cs
	namespace 	
Client
 
. 
Core 
{		 
[

 
CallbackBehavior

 
(

 
ConcurrencyMode

 %
=

& '
ConcurrencyMode

( 7
.

7 8
Multiple

8 @
,

@ A%
UseSynchronizationContext

B [
=

\ ]
false

^ c
)

c d
]

d e
public 

class 
GameServiceManager #
:$ %%
IGameLobbyServiceCallback& ?
{ 
private 
static 
GameServiceManager )
	_instance* 3
;3 4
public 
static 
GameServiceManager (
Instance) 1
=>2 4
	_instance5 >
??? A
(B C
	_instanceC L
=M N
newO R
GameServiceManagerS e
(e f
)f g
)g h
;h i
public "
GameLobbyServiceClient %
Client& ,
{- .
get/ 2
;2 3
private4 ;
set< ?
;? @
}A B
public 
event 
Action 
< 
string "
," #
string$ *
,* +
bool, 0
>0 1
ChatMessageReceived2 E
;E F
public 
event 
Action 
< 
string "
," #
bool$ (
>( )
PlayerJoined* 6
;6 7
public 
event 
Action 
< 
string "
>" #

PlayerLeft$ .
;. /
public 
event 
Action 
< 
LobbyPlayerInfo +
[+ ,
], -
>- .
PlayerListUpdated/ @
;@ A
public 
event 
Action 
< 
List  
<  !
CardInfo! )
>) *
>* +
GameStarted, 7
;7 8
public 
event 
Action 
< 
string "
," #
int$ '
>' (
TurnUpdated) 4
;4 5
public 
event 
Action 
< 
int 
,  
string! '
>' (
	CardShown) 2
;2 3
public   
event   
Action   
<   
int   
,    
int  ! $
>  $ %
CardsHidden  & 1
;  1 2
public!! 
event!! 
Action!! 
<!! 
int!! 
,!!  
int!!! $
>!!$ %
CardsMatched!!& 2
;!!2 3
public"" 
event"" 
Action"" 
<"" 
string"" "
,""" #
int""$ '
>""' (
ScoreUpdated"") 5
;""5 6
public## 
event## 
Action## 
<## 
string## "
>##" #
GameFinished##$ 0
;##0 1
private'' 
GameServiceManager'' "
(''" #
)''# $
{(( 	
InitializeClient)) 
()) 
))) 
;)) 
}** 	
private,, 
void,, 
InitializeClient,, %
(,,% &
),,& '
{-- 	
InstanceContext.. 
context.. #
=..$ %
new..& )
InstanceContext..* 9
(..9 :
this..: >
)..> ?
;..? @
Client// 
=// 
new// "
GameLobbyServiceClient// /
(/// 0
context//0 7
)//7 8
;//8 9
}00 	
public55 
async55 
Task55 
FlipCardAsync55 '
(55' (
int55( +
	cardIndex55, 5
)555 6
{66 	
if77 
(77 
Client77 
!=77 
null77 
&&77 !
Client77" (
.77( )
State77) .
==77/ 1
CommunicationState772 D
.77D E
Opened77E K
)77K L
{88 
await99 
Client99 
.99 
FlipCardAsync99 *
(99* +
	cardIndex99+ 4
)994 5
;995 6
}:: 
};; 	
public?? 
void?? 
ReceiveChatMessage?? &
(??& '
string??' -

senderName??. 8
,??8 9
string??: @
message??A H
,??H I
bool??J N
isNotification??O ]
)??] ^
{@@ 	
ChatMessageReceivedAA 
?AA  
.AA  !
InvokeAA! '
(AA' (

senderNameAA( 2
,AA2 3
messageAA4 ;
,AA; <
isNotificationAA= K
)AAK L
;AAL M
}BB 	
voidDD %
IGameLobbyServiceCallbackDD &
.DD& '
PlayerJoinedDD' 3
(DD3 4
stringDD4 :

playerNameDD; E
,DDE F
boolDDG K
isGuestDDL S
)DDS T
{EE 	
PlayerJoinedFF 
?FF 
.FF 
InvokeFF  
(FF  !

playerNameFF! +
,FF+ ,
isGuestFF- 4
)FF4 5
;FF5 6
}GG 	
voidII %
IGameLobbyServiceCallbackII &
.II& '

PlayerLeftII' 1
(II1 2
stringII2 8

playerNameII9 C
)IIC D
{JJ 	

PlayerLeftKK 
?KK 
.KK 
InvokeKK 
(KK 

playerNameKK )
)KK) *
;KK* +
}LL 	
publicNN 
voidNN 
UpdatePlayerListNN $
(NN$ %
LobbyPlayerInfoNN% 4
[NN4 5
]NN5 6
playersNN7 >
)NN> ?
{OO 	
PlayerListUpdatedPP 
?PP 
.PP 
InvokePP %
(PP% &
playersPP& -
)PP- .
;PP. /
}QQ 	
voidSS %
IGameLobbyServiceCallbackSS &
.SS& '
GameStartedSS' 2
(SS2 3
CardInfoSS3 ;
[SS; <
]SS< =
	gameBoardSS> G
)SSG H
{TT 	
GameStartedUU 
?UU 
.UU 
InvokeUU 
(UU  
	gameBoardUU  )
.UU) *
ToListUU* 0
(UU0 1
)UU1 2
)UU2 3
;UU3 4
}VV 	
publicXX 
voidXX 

UpdateTurnXX 
(XX 
stringXX %

playerNameXX& 0
,XX0 1
intXX2 5
turnTimeInSecondsXX6 G
)XXG H
{YY 	
TurnUpdatedZZ 
?ZZ 
.ZZ 
InvokeZZ 
(ZZ  

playerNameZZ  *
,ZZ* +
turnTimeInSecondsZZ, =
)ZZ= >
;ZZ> ?
}[[ 	
public]] 
void]] 
ShowCard]] 
(]] 
int]]  
	cardIndex]]! *
,]]* +
string]], 2
imageIdentifier]]3 B
)]]B C
{^^ 	
	CardShown__ 
?__ 
.__ 
Invoke__ 
(__ 
	cardIndex__ '
,__' (
imageIdentifier__) 8
)__8 9
;__9 :
}`` 	
publicbb 
voidbb 
	HideCardsbb 
(bb 
intbb !

cardIndex1bb" ,
,bb, -
intbb. 1

cardIndex2bb2 <
)bb< =
{cc 	
CardsHiddendd 
?dd 
.dd 
Invokedd 
(dd  

cardIndex1dd  *
,dd* +

cardIndex2dd, 6
)dd6 7
;dd7 8
}ee 	
voidgg %
IGameLobbyServiceCallbackgg &
.gg& '
CardFlippedgg' 2
(gg2 3
intgg3 6
	cardIndexgg7 @
,gg@ A
intggB E

cardIndex2ggF P
)ggP Q
{hh 	
}ii 	
publickk 
voidkk 
SetCardsAsMatchedkk %
(kk% &
intkk& )

cardIndex1kk* 4
,kk4 5
intkk6 9

cardIndex2kk: D
)kkD E
{ll 	
CardsMatchedmm 
?mm 
.mm 
Invokemm  
(mm  !

cardIndex1mm! +
,mm+ ,

cardIndex2mm- 7
)mm7 8
;mm8 9
}nn 	
publicpp 
voidpp 
UpdateScorepp 
(pp  
stringpp  &

playerNamepp' 1
,pp1 2
intpp3 6
newScorepp7 ?
)pp? @
{qq 	
ScoreUpdatedrr 
?rr 
.rr 
Invokerr  
(rr  !

playerNamerr! +
,rr+ ,
newScorerr- 5
)rr5 6
;rr6 7
}ss 	
voiduu %
IGameLobbyServiceCallbackuu &
.uu& '
GameFinisheduu' 3
(uu3 4
stringuu4 :

winnerNameuu; E
)uuE F
{vv 	
GameFinishedww 
?ww 
.ww 
Invokeww  
(ww  !

winnerNameww! +
)ww+ ,
;ww, -
}xx 	
}{{ 
}|| Ü
3C:\MemoryGame\Client\Client\Models\GameConstants.cs
	namespace 	
Client
 
. 
Models 
{ 
public 

static 
class 
GameConstants %
{ 
public 
const 
int 
DefaultCardCount )
=* +
$num, .
;. /
public 
const 
int "
DefaultTurnTimeSeconds /
=0 1
$num2 4
;4 5
public		 
const		 
int		 
MinCardCount		 %
=		& '
$num		( )
;		) *
public

 
const

 
int

 
MaxCardCount

 %
=

& '
$num

( *
;

* +
public 
const 
int 
MinTurnTimeSeconds +
=, -
$num. /
;/ 0
public 
const 
int &
MinTurnTimeSecondsFallback 3
=4 5
$num6 8
;8 9
public 
const 
string "
ColorCardFrontBasePath 2
=3 4
$str5 m
;m n
public 
const 
string #
NormalCardFrontBasePath 3
=4 5
$str6 o
;o p
public 
const 
string 
CardBackPath (
=) *
$str+ d
;d e
public 
const 
int 
MatchFeedbackDelay +
=, -
$num. 1
;1 2
public 
const 
int !
MismatchFeedbackDelay .
=/ 0
$num1 5
;5 6
public 
const 
int 
PointsPerMatch '
=( )
$num* ,
;, -
public   
const   
int   
MinPlayersToPlay   )
=  * +
$num  , -
;  - .
public!! 
const!! 
int!! 
MaxPlayersPerLobby!! +
=!!, -
$num!!. /
;!!/ 0
}## 
}$$ Ç'
/C:\MemoryGame\Client\Client\Core\UserSession.cs
	namespace

 	
Client


 
.

 
Core

 
{ 
internal 
static 
class 
UserSession %
{ 
public 
static 
string 
SessionToken )
{* +
get, /
;/ 0
set1 4
;4 5
}6 7
public 
static 
int 
UserId  
{! "
get# &
;& '
private( /
set0 3
;3 4
}5 6
public 
static 
string 
Username %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
public 
static 
bool 
IsGuest "
{# $
get% (
;( )
set* -
;- .
}/ 0
public 
static 
string 
Email "
{# $
get% (
;( )
private* 1
set2 5
;5 6
}7 8
public 
static 
string 
Name !
{" #
get$ '
;' (
set) ,
;, -
}. /
=0 1
string2 8
.8 9
Empty9 >
;> ?
public 
static 
string 
LastName %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
=4 5
string6 <
.< =
Empty= B
;B C
public 
static 
DateTime 
RegistrationDate /
{0 1
get2 5
;5 6
set7 :
;: ;
}< =
public 
static 
List 
< 
SocialNetworkDTO +
>+ ,
SocialNetworks- ;
{< =
get> A
;A B
setC F
;F G
}H I
=J K
newL O
ListP T
<T U
SocialNetworkDTOU e
>e f
(f g
)g h
;h i
public 
static 
void 
StartSession '
(' (
string( .
token/ 4
,4 5
UserDTO6 =
user> B
)B C
{ 	
if 
( 
user 
== 
null 
) 
throw #
new$ '!
ArgumentNullException( =
(= >
nameof> D
(D E
userE I
)I J
)J K
;K L
SessionToken 
= 
token  
;  !
UserId 
= 
user 
. 
UserId  
;  !
Username 
= 
user 
. 
Username $
;$ %
IsGuest   
=   
user   
.   
IsGuest   "
;  " #
Email!! 
=!! 
user!! 
.!! 
Email!! 
;!! 
Name"" 
="" 
user"" 
."" 
Name"" 
;"" 
LastName## 
=## 
user## 
.## 
LastName## $
;##$ %
RegistrationDate$$ 
=$$ 
user$$ #
.$$# $
RegistrationDate$$$ 4
;$$4 5
SocialNetworks%% 
=%% 
user%% !
.%%! "
SocialNetworks%%" 0
!=%%1 3
null%%4 8
?&& 
new&& 
List&& 
<&& 
SocialNetworkDTO&& +
>&&+ ,
(&&, -
user&&- 1
.&&1 2
SocialNetworks&&2 @
)&&@ A
:'' 
new'' 
List'' 
<'' 
SocialNetworkDTO'' +
>''+ ,
('', -
)''- .
;''. /
}(( 	
public** 
static** 
void** 

EndSession** %
(**% &
)**& '
{++ 	
SessionToken,, 
=,, 
null,, 
;,,  
UserId-- 
=-- 
$num-- 
;-- 
Username.. 
=.. 
null.. 
;.. 
Email// 
=// 
null// 
;// 
IsGuest00 
=00 
false00 
;00 
}11 	
public33 
static33 
event33 
Action33 "
ProfileUpdated33# 1
;331 2
public44 
static44 
void44 
OnProfileUpdated44 +
(44+ ,
)44, -
{55 	
ProfileUpdated66 
?66 
.66 
Invoke66 "
(66" #
)66# $
;66$ %
}77 	
}88 
}99 ”9
6C:\MemoryGame\Client\Client\Core\UserServiceManager.cs
	namespace		 	
Client		
 
.		 
Core		 
{

 
[ 
CallbackBehavior 
( 
ConcurrencyMode %
=& '
ConcurrencyMode( 7
.7 8
Multiple8 @
,@ A%
UseSynchronizationContextB [
=\ ]
false^ c
)c d
]d e
public 

class 
UserServiceManager #
:$ % 
IUserServiceCallback& :
{ 
private 
static 
UserServiceManager )
	_instance* 3
;3 4
public 
static 
UserServiceManager (
Instance) 1
=>2 4
	_instance5 >
??? A
(B C
	_instanceC L
=M N
newO R
UserServiceManagerS e
(e f
)f g
)g h
;h i
public 
UserServiceClient  
Client! '
{( )
get* -
;- .
private/ 6
set7 :
;: ;
}< =
private 
UserServiceManager "
(" #
)# $
{ 	
InitializeClient 
( 
) 
; 
} 	
private 
void 
InitializeClient %
(% &
)& '
{ 	
try 
{ 
if 
( 
Client 
!= 
null "
)" #
{ 
try   
{!! 
Client"" 
."" 
Close"" $
(""$ %
)""% &
;""& '
}## 
catch$$ 
{%% 
Client&& 
.&& 
Abort&& $
(&&$ %
)&&% &
;&&& '
}'' 
}(( 
InstanceContext** 
context**  '
=**( )
new*** -
InstanceContext**. =
(**= >
this**> B
)**B C
;**C D
Client++ 
=++ 
new++ 
UserServiceClient++ .
(++. /
context++/ 6
)++6 7
;++7 8
},, 
catch-- 
(-- 
	Exception-- 
ex-- 
)--  
{.. 
System// 
.// 
Diagnostics// "
.//" #
Debug//# (
.//( )
	WriteLine//) 2
(//2 3
$"//3 5
$str//5 W
{//W X
ex//X Z
.//Z [
Message//[ b
}//b c
"//c d
)//d e
;//e f
}00 
}11 	
public55 
async55 
Task55 
<55 
LoginResponse55 '
>55' (

LoginAsync55) 3
(553 4
string554 :
email55; @
,55@ A
string55B H
password55I Q
)55Q R
{66 	
if77 
(77 
EnsureConnection77  
(77  !
)77! "
)77" #
{88 
return99 
await99 
Client99 #
.99# $

LoginAsync99$ .
(99. /
email99/ 4
,994 5
password996 >
)99> ?
;99? @
}:: 
return;; 
new;; 
LoginResponse;; $
{;;% &
Success;;' .
=;;/ 0
false;;1 6
,;;6 7

MessageKey;;8 B
=;;C D
$str;;E b
};;c d
;;;d e
}<< 	
public>> 
async>> 
Task>> 
<>> 
LoginResponse>> '
>>>' (
RenewSessionAsync>>) :
(>>: ;
string>>; A
token>>B G
)>>G H
{?? 	
if@@ 
(@@ 
EnsureConnection@@  
(@@  !
)@@! "
)@@" #
{AA 
returnBB 
awaitBB 
ClientBB #
.BB# $
RenewSessionAsyncBB$ 5
(BB5 6
tokenBB6 ;
)BB; <
;BB< =
}CC 
returnDD 
newDD 
LoginResponseDD $
{DD% &
SuccessDD' .
=DD/ 0
falseDD1 6
}DD7 8
;DD8 9
}EE 	
privateGG 
boolGG 
EnsureConnectionGG %
(GG% &
)GG& '
{HH 	
ifII 
(II 
ClientII 
==II 
nullII 
||II !
ClientJJ 
.JJ 
StateJJ 
==JJ 
CommunicationStateJJ  2
.JJ2 3
ClosedJJ3 9
||JJ: <
ClientKK 
.KK 
StateKK 
==KK 
CommunicationStateKK  2
.KK2 3
FaultedKK3 :
)KK: ;
{LL 
InitializeClientMM  
(MM  !
)MM! "
;MM" #
}NN 
returnOO 
ClientOO 
.OO 
StateOO 
==OO  "
CommunicationStateOO# 5
.OO5 6
OpenedOO6 <
||OO= ?
ClientOO@ F
.OOF G
StateOOG L
==OOM O
CommunicationStateOOP b
.OOb c
CreatedOOc j
;OOj k
}PP 	
publicVV 
voidVV 
ForceLogoutVV 
(VV  
stringVV  &
reasonVV' -
)VV- .
{WW 	
ApplicationXX 
.XX 
CurrentXX 
.XX  

DispatcherXX  *
.XX* +
InvokeXX+ 1
(XX1 2
(XX2 3
)XX3 4
=>XX5 7
{YY 
ifZZ 
(ZZ 
ApplicationZZ 
.ZZ  
CurrentZZ  '
.ZZ' (

MainWindowZZ( 2
isZZ3 5
LoginZZ6 ;
)ZZ; <
{[[ 
return\\ 
;\\ 
}]] 

MessageBox__ 
.__ 
Show__ 
(__  
$"`` 
$str`` 2
{``2 3
reason``3 9
}``9 :
"``: ;
,``; <
$straa #
,aa# $
MessageBoxButtonbb $
.bb$ %
OKbb% '
,bb' (
MessageBoxImagebb) 8
.bb8 9
Warningbb9 @
)bb@ A
;bbA B
UserSessiondd 
.dd 

EndSessiondd &
(dd& '
)dd' (
;dd( )
ifff 
(ff 
Applicationff 
.ff  
Currentff  '
.ff' (

MainWindowff( 2
!=ff3 5
nullff6 :
)ff: ;
{gg 
NavigationHelperhh $
.hh$ %

NavigateTohh% /
(hh/ 0
Applicationhh0 ;
.hh; <
Currenthh< C
.hhC D

MainWindowhhD N
,hhN O
newhhP S
LoginhhT Y
(hhY Z
)hhZ [
)hh[ \
;hh\ ]
}ii 
elsejj 
{kk 
varmm 
loginmm 
=mm 
newmm  #
Loginmm$ )
(mm) *
)mm* +
;mm+ ,
Applicationnn 
.nn  
Currentnn  '
.nn' (

MainWindownn( 2
=nn3 4
loginnn5 :
;nn: ;
loginoo 
.oo 
Showoo 
(oo 
)oo  
;oo  !
}pp 
}qq 
)qq 
;qq 
}rr 	
}uu 
}vv ‡
LC:\MemoryGame\Client\Client\Core\Converters\SelfReportVisibilityConverter.cs
	namespace 	
Client
 
. 
Core 
. 

Converters  
{ 
public 

class )
SelfReportVisibilityConverter .
:/ 0
IValueConverter1 @
{		 
public

 
object

 
Convert

 
(

 
object

 $
value

% *
,

* +
Type

, 0

targetType

1 ;
,

; <
object

= C
	parameter

D M
,

M N
CultureInfo

O Z
culture

[ b
)

b c
{ 	
if 
( 
value 
is 
string 

winnerName  *
&&+ -

winnerName. 8
==9 ;
UserSession< G
.G H
UsernameH P
)P Q
{ 
return 

Visibility %
.% &
	Collapsed& /
;/ 0
} 
return 

Visibility 
. 
Visible %
;% &
} 	
public 
object 
ConvertBack !
(! "
object" (
value) .
,. /
Type0 4

targetType5 ?
,? @
objectA G
	parameterH Q
,Q R
CultureInfoS ^
culture_ f
)f g
{ 	
throw 
new #
NotImplementedException -
(- .
). /
;/ 0
} 	
} 
} ¶
7C:\MemoryGame\Client\Client\Models\DifficultyPresets.cs
	namespace 	
Client
 
. 
Models 
{		 
internal

 
static

 
class

 
DifficultyPresets

 +
{ 
public 
static 
GameConfiguration '
Easy( ,
=>- /
new0 3
GameConfiguration4 E
{ 	
NumberOfCards 
= 
$num 
, 
TimeLimitSeconds 
= 
$num !
,! "

NumberRows 
= 
$num 
, 
NumberColumns 
= 
$num 
, 
DifficultyLevel 
= 
Lang "
." #(
SelectDifficulty_Button_Easy# ?
} 	
;	 

public 
static 
GameConfiguration '
Normal( .
=>/ 1
new2 5
GameConfiguration6 G
{ 	
NumberOfCards 
= 
$num 
, 
TimeLimitSeconds 
= 
$num !
,! "

NumberRows 
= 
$num 
, 
NumberColumns 
= 
$num 
, 
DifficultyLevel 
= 
Lang "
." #*
SelectDifficulty_Button_Normal# A
} 	
;	 

public 
static 
GameConfiguration '
Hard( ,
=>- /
new0 3
GameConfiguration4 E
{ 	
NumberOfCards   
=   
$num   
,   
TimeLimitSeconds!! 
=!! 
$num!! "
,!!" #

NumberRows"" 
="" 
$num"" 
,"" 
NumberColumns## 
=## 
$num## 
,## 
DifficultyLevel$$ 
=$$ 
Lang$$ "
.$$" #(
SelectDifficulty_Button_Hard$$# ?
}%% 	
;%%	 

public'' 
static'' 
('' 
int'' 
Rows'' 
,''  
int''! $
Columns''% ,
)'', -
CalculateLayout''. =
(''= >
int''> A
numberOfCards''B O
)''O P
{(( 	
switch)) 
()) 
numberOfCards)) !
)))! "
{** 
case++ 
$num++ 
:++ 
return,, 
(,, 
$num,, 
,,, 
$num,,  
),,  !
;,,! "
case.. 
$num.. 
:.. 
return// 
(// 
$num// 
,// 
$num//  
)//  !
;//! "
case11 
$num11 
:11 
return22 
(22 
$num22 
,22 
$num22  
)22  !
;22! "
case44 
$num44 
:44 
default55 
:55 
return66 
(66 
$num66 
,66 
$num66  
)66  !
;66! "
}77 
}88 	
}99 
}:: ¥
4C:\MemoryGame\Client\Client\Helpers\WpfExtensions.cs
	namespace 	
Client
 
. 
Helpers 
{ 
public 

static 
class 
WpfExtensions %
{ 
public 
static 
async 
void  
SafeExecute! ,
(, -
this- 1
Button2 8
button9 ?
,? @
FuncA E
<E F
TaskF J
>J K
actionL R
,R S
WindowT Z
owner[ `
)` a
{ 	
if 
( 
button 
== 
null 
) 
{ 
return 
; 
} 
button 
. 
	IsEnabled 
= 
false $
;$ %
var 
originalCursor 
=  
Mouse! &
.& '
OverrideCursor' 5
;5 6
Mouse 
. 
OverrideCursor  
=! "
Cursors# *
.* +
Wait+ /
;/ 0
try 
{ 
await 
action 
( 
) 
; 
} 
catch 
( 
	Exception 
ex 
)  
{ 
ExceptionManager    
.    !
Handle  ! '
(  ' (
ex  ( *
,  * +
owner  , 1
)  1 2
;  2 3
}!! 
finally"" 
{## 
if$$ 
($$ 
Application$$ 
.$$  
Current$$  '
.$$' (

MainWindow$$( 2
==$$3 5
owner$$6 ;
)$$; <
{%% 
button&& 
.&& 
	IsEnabled&& $
=&&% &
true&&' +
;&&+ ,
}'' 
Mouse(( 
.(( 
OverrideCursor(( $
=((% &
originalCursor((' 5
;((5 6
})) 
}** 	
}++ 
},, ÍN
4C:\MemoryGame\Client\Client\Core\ExceptionManager.cs
	namespace

 	
Client


 
.

 
Core

 
{ 
public 

static 
class 
ExceptionManager (
{ 
private 
static 
readonly 
object  &
_logLock' /
=0 1
new2 5
object6 <
(< =
)= >
;> ?
private 
const 
string 
LogFileName (
=) *
$str+ A
;A B
public 
static 
void 
Handle !
(! "
	Exception" +
ex, .
,. /
Window0 6
owner7 <
== >
null? C
,C D
ActionE K
	onHandledL U
=V W
nullX \
,\ ]
bool^ b
isFatalc j
=k l
falsem r
)r s
{ 	
if 
( 
ex 
== 
null 
) 
{ 
return 
; 
} 
LogException 
( 
ex 
) 
; 
var!! 
details!! 
=!! 
GetExceptionDetails!! -
(!!- .
ex!!. 0
)!!0 1
;!!1 2
string"" 
title"" 
="" 
details"" "
.""" #
Title""# (
;""( )
string## 
message## 
=## 
details## $
.##$ %
Message##% ,
;##, -
if%% 
(%% 
isFatal%% 
)%% 
{&& 
message'' 
+='' 
$"'' 
$str'' !
{''! "
Lang''" &
.''& '#
Global_Error_FatalCrash''' >
}''> ?
"''? @
;''@ A
}(( 
if** 
(** 
Application** 
.** 
Current** #
==**$ &
null**' +
)**+ ,
{++ 
return,, 
;,, 
}-- 
Application// 
.// 
Current// 
.//  

Dispatcher//  *
.//* +
Invoke//+ 1
(//1 2
(//2 3
)//3 4
=>//5 7
{00 
try11 
{22 
if33 
(33 
owner33 
==33  
null33! %
&&33& (
Application33) 4
.334 5
Current335 <
.33< =

MainWindow33= G
!=33H J
null33K O
&&33P R
Application33S ^
.33^ _
Current33_ f
.33f g

MainWindow33g q
.33q r
	IsVisible33r {
)33{ |
{44 
owner55 
=55 
Application55  +
.55+ ,
Current55, 3
.553 4

MainWindow554 >
;55> ?
}66 
var88 
type88 
=88 
isFatal88 &
?88' (
CustomMessageBox88) 9
.889 :
MessageBoxType88: H
.88H I
Error88I N
:88O P
CustomMessageBox88Q a
.88a b
MessageBoxType88b p
.88p q
Warning88q x
;88x y
CustomMessageBox:: $

messageBox::% /
;::/ 0
try;; 
{<< 

messageBox== "
===# $
new==% (
CustomMessageBox==) 9
(==9 :
title==: ?
,==? @
message==A H
,==H I
owner==J O
,==O P
type==Q U
)==U V
;==V W
}>> 
catch?? 
{@@ 

messageBoxAA "
=AA# $
newAA% (
CustomMessageBoxAA) 9
(AA9 :
titleAA: ?
,AA? @
messageAAA H
,AAH I
nullAAJ N
,AAN O
typeAAP T
)AAT U
;AAU V
}BB 

messageBoxDD 
.DD 

ShowDialogDD )
(DD) *
)DD* +
;DD+ ,
	onHandledFF 
?FF 
.FF 
InvokeFF %
(FF% &
)FF& '
;FF' (
ifHH 
(HH 
isFatalHH 
)HH  
{II 
UserSessionJJ #
.JJ# $

EndSessionJJ$ .
(JJ. /
)JJ/ 0
;JJ0 1
ApplicationKK #
.KK# $
CurrentKK$ +
.KK+ ,
ShutdownKK, 4
(KK4 5
)KK5 6
;KK6 7
}LL 
}MM 
catchNN 
(NN 
	ExceptionNN  

dispatchExNN! +
)NN+ ,
{OO 
DebugPP 
.PP 
	WriteLinePP #
(PP# $
$"PP$ &
$strPP& G
{PPG H

dispatchExPPH R
.PPR S
MessagePPS Z
}PPZ [
"PP[ \
)PP\ ]
;PP] ^
}QQ 
}RR 
)RR 
;RR 
}SS 	
privateUU 
staticUU 
(UU 
stringUU 
TitleUU $
,UU$ %
stringUU& ,
MessageUU- 4
)UU4 5
GetExceptionDetailsUU6 I
(UUI J
	ExceptionUUJ S
exUUT V
)UUV W
{VV 	
stringWW 
titleWW 
=WW 
LangWW 
.WW  !
Global_Title_AppErrorWW  5
;WW5 6
stringXX 
messageXX 
=XX 
LocalizationHelperXX /
.XX/ 0
	GetStringXX0 9
(XX9 :
exXX: <
)XX< =
;XX= >
ifZZ 
(ZZ 
exZZ 
isZZ %
EndpointNotFoundExceptionZZ /
||ZZ0 2
exZZ3 5
isZZ6 8"
ServerTooBusyExceptionZZ9 O
)ZZO P
{[[ 
title\\ 
=\\ 
Lang\\ 
.\\ &
Global_Title_ServerOffline\\ 7
;\\7 8
message]] 
=]] 
Lang]] 
.]] )
Global_Label_ConnectionFailed]] <
;]]< =
}^^ 
else__ 
if__ 
(__ 
ex__ 
is__ 
TimeoutException__ +
||__, .
ex__/ 1
is__2 4"
CommunicationException__5 K
)__K L
{`` 
titleaa 
=aa 
Langaa 
.aa %
Global_Title_NetworkErroraa 6
;aa6 7
messagebb 
=bb 
LocalizationHelperbb ,
.bb, -
	GetStringbb- 6
(bb6 7
exbb7 9
)bb9 :
;bb: ;
}cc 
elsedd 
ifdd 
(dd 
exdd 
isdd 
FaultExceptiondd )
faultExdd* 1
)dd1 2
{ee 
titleff 
=ff 
Langff 
.ff 
Global_Title_Errorff /
;ff/ 0
messagegg 
=gg 
LocalizationHelpergg ,
.gg, -
	GetStringgg- 6
(gg6 7
faultExgg7 >
.gg> ?
Messagegg? F
)ggF G
;ggG H
}hh 
elseii 
ifii 
(ii 
exii 
isii '
UnauthorizedAccessExceptionii 6
)ii6 7
{jj 
titlekk 
=kk 
Langkk 
.kk 
Global_Title_Errorkk /
;kk/ 0
messagell 
=ll 
Langll 
.ll %
Global_Error_AccessDeniedll 8
;ll8 9
}mm 
returnoo 
(oo 
titleoo 
,oo 
messageoo "
)oo" #
;oo# $
}pp 	
privaterr 
staticrr 
voidrr 
LogExceptionrr (
(rr( )
	Exceptionrr) 2
exrr3 5
)rr5 6
{ss 	
trytt 
{uu 
stringvv 
logPathvv 
=vv  
Pathvv! %
.vv% &
Combinevv& -
(vv- .
	AppDomainvv. 7
.vv7 8
CurrentDomainvv8 E
.vvE F
BaseDirectoryvvF S
,vvS T
LogFileNamevvU `
)vv` a
;vva b
stringww 

logContentww !
=ww" #
$"ww$ &
$strww& '
{ww' (
DateTimeww( 0
.ww0 1
Nowww1 4
:ww4 5
$strww5 H
}wwH I
$strwwI L
{wwL M
exwwM O
.wwO P
GetTypewwP W
(wwW X
)wwX Y
.wwY Z
NamewwZ ^
}ww^ _
$strww_ b
{wwb c
exwwc e
.wwe f
Messagewwf m
}wwm n
$strwwn p
"wwp q
+wwr s
$"xx$ &
$strxx& 4
{xx4 5
exxx5 7
.xx7 8

StackTracexx8 B
}xxB C
$strxxC E
"xxE F
+xxG H
$"yy$ &
$stryy& 7
{yy7 8
exyy8 :
.yy: ;
InnerExceptionyy; I
?yyI J
.yyJ K
MessageyyK R
??yyS U
$stryyV \
}yy\ ]
$stryy] _
"yy_ `
+yya b
$strzz$ Z
;zzZ [
lock|| 
(|| 
_logLock|| 
)|| 
{}} 
File~~ 
.~~ 
AppendAllText~~ &
(~~& '
logPath~~' .
,~~. /

logContent~~0 :
)~~: ;
;~~; <
} 
Debug
ÅÅ 
.
ÅÅ 
	WriteLine
ÅÅ 
(
ÅÅ  
$"
ÅÅ  "
$str
ÅÅ" @
{
ÅÅ@ A
logPath
ÅÅA H
}
ÅÅH I
"
ÅÅI J
)
ÅÅJ K
;
ÅÅK L
}
ÇÇ 
catch
ÉÉ 
(
ÉÉ 
	Exception
ÉÉ 
logEx
ÉÉ "
)
ÉÉ" #
{
ÑÑ 
Debug
ÖÖ 
.
ÖÖ 
	WriteLine
ÖÖ 
(
ÖÖ  
$"
ÖÖ  "
$str
ÖÖ" G
{
ÖÖG H
logEx
ÖÖH M
.
ÖÖM N
Message
ÖÖN U
}
ÖÖU V
"
ÖÖV W
)
ÖÖW X
;
ÖÖX Y
}
ÜÜ 
}
áá 	
}
àà 
}ââ Ò8
2C:\MemoryGame\Client\Client\Helpers\ImageHelper.cs
	namespace 	
Client
 
. 
Helpers 
{ 
public 

static 
class 
ImageHelper #
{ 
private 
const 
int 
MAX_IMAGE_SIZE (
=) *
$num+ ,
*- .
$num/ 3
*4 5
$num6 :
;: ;
public 
static 
BitmapImage !"
ByteArrayToImageSource" 8
(8 9
byte9 =
[= >
]> ?
	imageData@ I
)I J
{ 	
if 
( 
	imageData 
== 
null !
||" $
	imageData% .
.. /
Length/ 5
==6 8
$num9 :
): ;
{ 
return 
null 
; 
} 
var 
image 
= 
new 
BitmapImage '
(' (
)( )
;) *
image 
. 
	BeginInit 
( 
) 
; 
image 
. 
StreamSource 
=  
new! $
System% +
.+ ,
IO, .
.. /
MemoryStream/ ;
(; <
	imageData< E
)E F
;F G
image 
. 
CacheOption 
= 
BitmapCacheOption  1
.1 2
OnLoad2 8
;8 9
image 
. 
EndInit 
( 
) 
; 
image 
. 
Freeze 
( 
) 
; 
return 
image 
; 
}   	
public"" 
static"" 
byte"" 
["" 
]"" "
ImageSourceToByteArray"" 3
(""3 4
BitmapImage""4 ?
image""@ E
)""E F
{## 	
if$$ 
($$ 
image$$ 
==$$ 
null$$ 
)$$ 
return$$ %
Array$$& +
.$$+ ,
Empty$$, 1
<$$1 2
byte$$2 6
>$$6 7
($$7 8
)$$8 9
;$$9 :
var&& 
encoder&& 
=&& 
new&& 
PngBitmapEncoder&& .
(&&. /
)&&/ 0
;&&0 1
encoder'' 
.'' 
Frames'' 
.'' 
Add'' 
('' 
BitmapFrame'' *
.''* +
Create''+ 1
(''1 2
image''2 7
)''7 8
)''8 9
;''9 :
using)) 
()) 
var)) 
stream)) 
=)) 
new))  #
MemoryStream))$ 0
())0 1
)))1 2
)))2 3
{** 
encoder++ 
.++ 
Save++ 
(++ 
stream++ #
)++# $
;++$ %
return,, 
stream,, 
.,, 
ToArray,, %
(,,% &
),,& '
;,,' (
}-- 
}.. 	
public00 
static00 
byte00 
[00 
]00 
ResizeImage00 (
(00( )
byte00) -
[00- .
]00. /
	imageData000 9
,009 :
int00; >
maxWidth00? G
,00G H
int00I L
	maxHeight00M V
)00V W
{11 	
if22 
(22 
	imageData22 
==22 
null22 !
||22" $
	imageData22% .
.22. /
Length22/ 5
==226 8
$num229 :
)22: ;
{33 
return44 
Array44 
.44 
Empty44 "
<44" #
byte44# '
>44' (
(44( )
)44) *
;44* +
}55 
if77 
(77 
	imageData77 
.77 
Length77  
>77! "
MAX_IMAGE_SIZE77# 1
)771 2
{88 
throw99 
new99 %
InvalidOperationException99 3
(993 4
$str994 E
)99E F
;99F G
}:: 
var<< 
originalImage<< 
=<< "
ByteArrayToImageSource<<  6
(<<6 7
	imageData<<7 @
)<<@ A
;<<A B
if== 
(== 
originalImage== 
====  
null==! %
)==% &
return==' -
Array==. 3
.==3 4
Empty==4 9
<==9 :
byte==: >
>==> ?
(==? @
)==@ A
;==A B
if?? 
(?? 
originalImage?? 
.?? 

PixelWidth?? (
>??) *
$num??+ /
||??0 2
originalImage??3 @
.??@ A
PixelHeight??A L
>??M N
$num??O S
)??S T
{@@ 
throwAA 
newAA %
InvalidOperationExceptionAA 3
(AA3 4
$strAA4 N
)AAN O
;AAO P
}BB 
doubleDD 
ratioXDD 
=DD 
(DD 
doubleDD #
)DD# $
maxWidthDD$ ,
/DD- .
originalImageDD/ <
.DD< =

PixelWidthDD= G
;DDG H
doubleEE 
ratioYEE 
=EE 
(EE 
doubleEE #
)EE# $
	maxHeightEE$ -
/EE. /
originalImageEE0 =
.EE= >
PixelHeightEE> I
;EEI J
doubleFF 
ratioFF 
=FF 
MathFF 
.FF  
MinFF  #
(FF# $
ratioXFF$ *
,FF* +
ratioYFF, 2
)FF2 3
;FF3 4
ifHH 
(HH 
ratioHH 
>=HH 
$numHH 
)HH 
returnHH "
	imageDataHH# ,
;HH, -
varJJ 
resizedJJ 
=JJ 
newJJ 
TransformedBitmapJJ /
(JJ/ 0
originalImageJJ0 =
,JJ= >
newKK 
SystemKK 
.KK 
WindowsKK "
.KK" #
MediaKK# (
.KK( )
ScaleTransformKK) 7
(KK7 8
ratioKK8 =
,KK= >
ratioKK? D
)KKD E
)KKE F
;KKF G
varMM 
encoderMM 
=MM 
newMM 
JpegBitmapEncoderMM /
{MM0 1
QualityLevelMM2 >
=MM? @
$numMMA C
}MMD E
;MME F
encoderNN 
.NN 
FramesNN 
.NN 
AddNN 
(NN 
BitmapFrameNN *
.NN* +
CreateNN+ 1
(NN1 2
resizedNN2 9
)NN9 :
)NN: ;
;NN; <
usingPP 
(PP 
varPP 
streamPP 
=PP 
newPP  #
MemoryStreamPP$ 0
(PP0 1
)PP1 2
)PP2 3
{QQ 
encoderRR 
.RR 
SaveRR 
(RR 
streamRR #
)RR# $
;RR$ %
returnSS 
streamSS 
.SS 
ToArraySS %
(SS% &
)SS& '
;SS' (
}TT 
}UU 	
}VV 
}WW ‚
2C:\MemoryGame\Client\Client\Helpers\EmailHelper.cs
	namespace 	
Client
 
. 
Helpers 
{ 
internal		 
static		 
class		 
EmailHelper		 %
{

 
public 
static 
bool 
isValidEmail '
(' (
string( .
email/ 4
)4 5
{ 	
try 
{ 
var 
address 
= 
new !
System" (
.( )
Net) ,
., -
Mail- 1
.1 2
MailAddress2 =
(= >
email> C
)C D
;D E
return 
address 
. 
Address &
==' )
email* /
;/ 0
} 
catch 
{ 
return 
false 
; 
} 
} 	
public 
static 
bool 
SendInvitationEmail .
(. /
string/ 5
email6 ;
,; <
string= C
gameCodeD L
)L M
{ 	
try 
{ 
var 

smtpClient 
=  
new! $
System% +
.+ ,
Net, /
./ 0
Mail0 4
.4 5

SmtpClient5 ?
(? @
$str@ R
)R S
{ 
Port 
= 
$num 
, 
Credentials 
=  !
new" %
System& ,
., -
Net- 0
.0 1
NetworkCredential1 B
(B C
$strC Q
,Q R
$strS a
)a b
,b c
	EnableSsl   
=   
true    $
,  $ %
}!! 
;!! 
var## 
mailMessage## 
=##  !
new##" %
System##& ,
.##, -
Net##- 0
.##0 1
Mail##1 5
.##5 6
MailMessage##6 A
{$$ 
From%% 
=%% 
new%% 
System%% %
.%%% &
Net%%& )
.%%) *
Mail%%* .
.%%. /
MailAddress%%/ :
(%%: ;
$str%%; I
)%%I J
,%%J K
Subject&& 
=&& 
$str&& 5
,&&5 6
Body'' 
='' 
$"'' 
$str'' i
{''i j
gameCode''j r
}''r s
"''s t
,''t u

IsBodyHtml(( 
=((  
false((! &
,((& '
})) 
;)) 
mailMessage** 
.** 
To** 
.** 
Add** "
(**" #
email**# (
)**( )
;**) *

smtpClient++ 
.++ 
Send++ 
(++  
mailMessage++  +
)+++ ,
;++, -
return,, 
true,, 
;,, 
}-- 
catch.. 
{// 
return00 
false00 
;00 
}11 
}22 	
}33 
}44 Ó.
7C:\MemoryGame\Client\Client\Helpers\NavigationHelper.cs
	namespace 	
Client
 
. 
Helpers 
{		 
public

 

static

 
class

 
NavigationHelper

 (
{ 
public 
static 
void 

NavigateTo %
(% &
Window& ,
currentWindow- :
,: ;
Window< B

nextWindowC M
)M N
{ 	
if 
( 
currentWindow 
==  
null! %
||& (

nextWindow) 3
==4 6
null7 ;
); <
{ 
return 
; 
} 
if 
( 
currentWindow 
. 
WindowState )
==* ,
WindowState- 8
.8 9
	Maximized9 B
)B C
{ 

nextWindow 
. 
WindowState &
=' (
WindowState) 4
.4 5
	Maximized5 >
;> ?
} 
else 
if 
( 
currentWindow "
." #
WindowState# .
==/ 1
WindowState2 =
.= >
Normal> D
)D E
{ 

nextWindow 
. 
WindowState &
=' (
currentWindow) 6
.6 7
WindowState7 B
;B C

nextWindow 
. 
Width  
=! "
currentWindow# 0
.0 1
Width1 6
;6 7

nextWindow 
. 
Height !
=" #
currentWindow$ 1
.1 2
Height2 8
;8 9

nextWindow 
. 
Top 
=  
currentWindow! .
.. /
Top/ 2
;2 3

nextWindow 
. 
Left 
=  !
currentWindow" /
./ 0
Left0 4
;4 5
} 

nextWindow 
. 
Show 
( 
) 
; 
Application   
.   
Current   
.    

MainWindow    *
=  + ,

nextWindow  - 7
;  7 8
List"" 
<"" 
Window"" 
>"" 
windowsToClose"" '
=""( )
Application""* 5
.""5 6
Current""6 =
.""= >
Windows""> E
.""E F
Cast""F J
<""J K
Window""K Q
>""Q R
(""R S
)""S T
.""T U
ToList""U [
(""[ \
)""\ ]
;""] ^
foreach$$ 
($$ 
var$$ 
window$$ 
in$$  "
windowsToClose$$# 1
)$$1 2
{%% 
if&& 
(&& 
window&& 
!=&& 

nextWindow&& (
)&&( )
{'' 
window(( 
.(( 
Close((  
(((  !
)((! "
;((" #
})) 
}** 
}++ 	
public-- 
static-- 
bool-- 
?-- 

ShowDialog-- &
(--& '
Window--' -
parentWindow--. :
,--: ;
Window--< B
dialogWindow--C O
)--O P
{.. 	
dialogWindow// 
.// 
Owner// 
=//  
parentWindow//! -
;//- .
dialogWindow00 
.00 
WindowState00 $
=00% &
parentWindow00' 3
.003 4
WindowState004 ?
;00? @
return11 
dialogWindow11 
.11  

ShowDialog11  *
(11* +
)11+ ,
;11, -
}22 	
public44 
static44 
OpenFileDialog44 $
GetOpenFileDialog44% 6
(446 7
string447 =
title44> C
,44C D
string44E K
filter44L R
,44R S
bool44T X
isMultiSelect44Y f
)44f g
{55 	
OpenFileDialog66 
dialog66 !
=66" #
new66$ '
OpenFileDialog66( 6
(666 7
)667 8
;668 9
dialog77 
.77 
Title77 
=77 
title77  
;77  !
dialog88 
.88 
Filter88 
=88 
filter88 "
;88" #
dialog99 
.99 
Multiselect99 
=99  
isMultiSelect99! .
;99. /
return:: 
dialog:: 
;:: 
};; 	
public== 
static== 
void== 
ExitApplication== *
(==* +
)==+ ,
{>> 	
try?? 
{@@ 
ifAA 
(AA 
UserSessionAA 
.AA  
IsGuestAA  '
)AA' (
{BB 
UserServiceManagerCC &
.CC& '
InstanceCC' /
.CC/ 0
ClientCC0 6
.CC6 7
LogoutGuestAsyncCC7 G
(CCG H
UserSessionCCH S
.CCS T
SessionTokenCCT `
)CC` a
;CCa b
}DD 
UserSessionEE 
.EE 

EndSessionEE &
(EE& '
)EE' (
;EE( )
}FF 
catchGG 
(GG 
	ExceptionGG 
exGG 
)GG  
{HH 
SystemII 
.II 
DiagnosticsII "
.II" #
DebugII# (
.II( )
	WriteLineII) 2
(II2 3
$"II3 5
$strII5 C
{IIC D
exIID F
}IIF G
"IIG H
)IIH I
;III J
}JJ 
finallyKK 
{LL 
ApplicationMM 
.MM 
CurrentMM #
.MM# $
ShutdownMM$ ,
(MM, -
)MM- .
;MM. /
}NN 
}OO 	
}PP 
}QQ ô
3C:\MemoryGame\Client\Client\Helpers\ClientHelper.cs
	namespace 	
Client
 
. 
Helpers 
{ 
internal		 
static		 
class		 
ClientHelper		 &
{

 
public 
static 
string 
GenerateGameCode -
(- .
). /
{ 	
var 
random 
= 
new 
Random #
(# $
)$ %
;% &
return 
random 
. 
Next 
( 
$num %
,% &
$num' -
)- .
.. /
ToString/ 7
(7 8
)8 9
;9 :
} 	
} 
} 