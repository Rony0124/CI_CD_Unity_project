# CI_CD_Unity_project

GitHub Actions를 사용하여 Unity 프로젝트의 CI/CD를 설정하는 간단한 튜토리얼

.github/workflows 폴더에 2개의 .yml 파일이 있을 것입니다. 첫 번째는 activation.yml입니다. 모든 액션은 Unity 설치를 사용하므로 활성화되어야 합니다. activation.yml은 game.ci의 문서에서 가져온 것이며 필요에 따라 수정할 수 있습니다.

main.yml은 자체 빌드 워크플로우를 설정할 수 있는 곳입니다. 에셋을 캐시하고, 빌드를 아티팩트에 업로드하며, Android 빌드를 생성하고 있습니다. 테스트 용 입니다.
![Screenshot 2023-06-21 at 4 16 59 PM](https://github.com/Rony0124/CI_CD_Unity_project/assets/56284745/95a5d8e1-b043-4a0f-8b0d-fc0f07c9ce08)
