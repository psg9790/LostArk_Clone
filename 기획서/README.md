
# 기획서
## 컨셉트
### 로스트아크 발탄 레이드 모작
클래스
- 기상술사

## 타이틀 씬
#### 로그인/ 회원가입
<u>*Firebase Auth*</u>로 구현한다.
씬 가운데에 **SignUp**, **SignIn** 버튼 두 개를 배치하고 각 버튼을 눌렀을 떄 해당하는 작업창이 뜨게 한다.  
- **SignUp**: email과 password를 입력받는 회원가입 창을 띄운다. 등록 결과(성공/실패/취소)를 사용자가 인지할 수 있도록 UI를 띄워준다.
- **SignIn**: email과 password를 입력받는 로그인 창을 띄운다. 시도 결과(성공/실패/취소)에 따라 성공 시 인게임으로 씬 전환, 실패 시 UI를 띄운다.  
</br>

## 로비 씬
#### 매치메이킹
모든 유저가 한곳에 모이는 로비.
방 공개를 통해 원하는 방에서 4명을 모아 레이드에 입장함.  
![](https://static.inven.co.kr/column/2023/02/03/news/i15778576269.png)  

## 레이드 씬
#### 카메라
<u>*Cinemachine*</u>을 이용한 쿼터 뷰를 구현.  
거리별 3단 카메라로 설계, 마우스 스크롤을 굴리는 것으로 조절 가능하도록 함. 

#### 조작
쿼터뷰에 맞게 우클릭으로 땅을 찍어 이동하는 조작

#### 실시간 채팅
<u>*Firebase Realtime Database*</u>로 구현  

#### 모델링
발탄 (몬스터)  
![발탄](https://img1.daumcdn.net/thumb/R1280x0/?scode=mtistory2&fname=https%3A%2F%2Fblog.kakaocdn.net%2Fdn%2FbEHIKO%2FbtspxcpoIs9%2F1xOdpaoEE8xow2Q0Ap7Kgk%2Fimg.webp)
레이드 맵  
![부활한 마수의 심장](https://i.namu.wiki/i/jq_X8f5nIeg-f4IR3NL4CGYtfzV66IxuMyRi2b8dwuMDR5vJER29JzUtrP-0chr8pOFk4PhBeU7jUR7bsEOv5OBT5GRcrL9LGJSpXPcHxudZHllSaLtWWF4fpQGgi00dNhap2KpNRr-v9LHbrEM17w.webp)  

#### 실시간 멀티플레이
<u>*Photon View*</u>를 이용
- 위치 및 애니메이션 동기화
- 이펙트 동기화  

![](https://d3kxs6kpbh59hp.cloudfront.net/community/COMMUNITY/2134147d66c24c69b2704b6069197d7a/5de5ea4b83c641e6b1b2ea8dcebb7916_1608214311.png)  
#### HP/ MP/ 스킬
화면 하단에 각종 스킬들과 HP/ MP UI를 띄워줌.
스킬들은 쿨타임까지 표시할 수 있도록 함.
![](https://static.inven.co.kr/column/2017/09/16/news/i13577284414.jpg)  
#### 타임어택
레이드 시간에 따른 광폭화 구현,
보스의 데미지가 쎄지고 HP를 회복하여 공략이 어려워짐.  
![](http://d3kxs6kpbh59hp.cloudfront.net/community/COMMUNITY/f1273958ee2e44fca6ad7b6ca3885a24/1a681d93f79840d28e120ab11cd649be_1527851806.png)  
#### 사망
죽은 후에도 관전을 할수 있도록 조작 변경  
한명을 포커싱하고 지켜볼 수 있는 화면.  
![](https://upload3.inven.co.kr/upload/2022/08/11/bbs/i14265731425.jpg?MW=800)  
#### 데이터 저장
<u>*Firebase Firestore*</u>로 저장
- 재화
- 보스 처치 횟수
- 사망 횟수
