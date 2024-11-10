#include <stdlib.h>
#include <time.h>
#include "printMap.h"

// 가중치를 초기화하는 함수
void initializeWeights(int weights[HEIGHT][WIDTH][4]) {
    srand(time(NULL));

    for (int i = 0; i < HEIGHT; i++) {
        for (int j = 0; j < WIDTH; j++) {
            if (j < WIDTH - 1) {
                // 오른쪽 가중치 설정
                int weight = rand() % 10; // 0에서 9 사이의 값으로 설정
                weights[i][j][0] = weight;        // 현재 셀의 오른쪽
                weights[i][j + 1][2] = weight;    // 오른쪽 셀의 왼쪽
            }
            else {
                weights[i][j][0] = 0; // 맵의 오른쪽 경계에서 끊김
            }

            if (i < HEIGHT - 1) {
                // 아래쪽 가중치 설정
                int weight = rand() % 10; // 0에서 9 사이의 값으로 설정
                weights[i][j][1] = weight;        // 현재 셀의 아래쪽
                weights[i + 1][j][3] = weight;    // 아래쪽 셀의 위쪽
            }
            else {
                weights[i][j][1] = 0; // 맵의 아래쪽 경계에서 끊김
            }

            // 맨 왼쪽과 맨 위 셀들은 경계와 연결되지 않으므로 0으로 설정
            if (j == 0) {
                weights[i][j][2] = 0; // 맨 왼쪽 경계
            }
            if (i == 0) {
                weights[i][j][3] = 0; // 맨 위 경계
            }
        }
    }
}

