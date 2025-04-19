# 🏃 EndRunner - Android 게임 프로젝트

### 👤 인원  
- 게임 프로그래머 1인 (2인 팀 프로젝트 / 프로그래밍 전담)

---

### 🎮 프로젝트 개요

**EndRunner**는 Android 기반의 1인칭 러너 게임으로, 친구와 함께 2인 팀으로 개발하였습니다.  
개발 이후 **Google Play Store에 비공개 출시하여 실제 기기에서 테스트 및 피드백**을 진행하였으며,  
실제 유저 플레이를 고려한 기능 적용 및 외부 서비스 연동 경험을 쌓는 데에 중점을 두었습니다.

![Image](https://github.com/user-attachments/assets/5798d6fc-b627-440b-b348-438f129d7b4e)

---

### ✨ 핵심 기능

#### 🔹 로그인 및 리더보드 기능  
- Google Play 게임 서비스(GPGS 0.10.14)와 Firebase를 활용한 로그인 처리  
- GPGS 리더보드 연동을 통해 **유저 간 경쟁 요소 구현**

![Image](https://github.com/user-attachments/assets/b6b68aa7-53ba-4201-9382-13cfdc53138d)
![Image](https://github.com/user-attachments/assets/18ce2d6f-ccff-49cc-a4e4-adf7816a3ffe)

#### 🔹 광고 시스템  
- Google AdMob 라이브러리를 사용하여 **보상형 광고** 송출 기능 구현  
- 게임 흐름을 방해하지 않도록 광고 위치 및 타이밍 설계

![Image](https://github.com/user-attachments/assets/38770ddd-a3ef-425c-a033-39e8ab9cb862)

#### 🔹 데이터 저장  
- SQL Lite를 활용하여 게임 내 설정, 점수 기록 등을 로컬 DB에 저장  
- 유저 데이터를 **지속적이고 안정적으로 관리**하기 위한 저장 구조 설계

![Image](https://github.com/user-attachments/assets/02aead61-679a-4559-b7bc-3f9bcb40906f)

#### 🔹 UI 구현  
- NGUI를 활용한 UI 구성  
- 게임 메뉴, 설정창, 인게임 HUD 등을 포함한 **전체 UI 작업을 직접 구현**

![Image](https://github.com/user-attachments/assets/9bd38927-93ed-457c-9929-aa4e5cc918e3)

---

### 🚀 배포 및 테스트
- Google Play Store 비공개 릴리스 버전으로 테스트 배포  
- 다양한 기기에서 테스트를 진행하며 **실제 환경 대응 능력 강화**

---

### 🔧 트러블슈팅 경험

#### 🔸 Gradle 빌드 오류 해결
- Android 빌드 과정에서 **Gradle 버전 불일치 및 환경 설정 문제로 인한 빌드 실패** 발생
- 다양한 해결 방법을 시도한 결과:
  1. 기존 Gradle 캐시 삭제 및 재설치
  2. Unity 내 Gradle 버전 수동 설정 (커스텀 Gradle 템플릿 적용)
  3. 프로젝트 설정 내 build.gradle 수정으로 라이브러리 호환성 문제 해결
- 문제 발생 원인을 분석하고, 커스텀 설정을 통해 **정상적인 Android 빌드 환경을 복구**

---

### 📁 기술 스택
- Unity 버전: [2022.3.17f1]
- C# 버전: [C# 8.0]
- GPGS 버전: [0.10.14]
- Firebase 버전: [12.8.0]
- Google AdMob 버전: [8.6.0]
- SQL Lite
- NGUI

---

### 📌 프로젝트 목표 요약
- 외부 서비스와의 연동을 통해 실제 배포 가능한 게임 구조 경험  
- Android 환경에서의 광고, 로그인, DB 등 실전 기능 직접 적용  
- 테스트와 피드백을 통해 **개발 후반부 대응력과 실행력 향상**
