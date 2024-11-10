#include <stdio.h>
#include "printMap.h"

void printGrid(int weights[HEIGHT][WIDTH][4]) {
    for (int i = 0; i < HEIGHT; i++) {
        // 가로 방향 출력
        printf(" ");
        for (int j = 0; j < WIDTH; j++) {
            printf("○");

            // 오른쪽 가중치 출력
            if (j < WIDTH - 1) {
                if (weights[i][j][0] != INF) {
                    printf("----%d----", weights[i][j][0]);
                }
                else {
                    printf("         "); // 가중치가 INF일 때는 빈 공간
                }
            }
        }
        printf("\n");

        // 세로 방향 막대기 출력
        if (i < HEIGHT - 1) {
            for (int j = 0; j < WIDTH; j++) {
                if (weights[i][j][1] != INF) {
                    printf(" |       ");
                }
                else {
                    printf("         "); // 가중치가 INF일 때는 빈 공간
                }

                if (j < WIDTH - 1) {
                    printf(" ");
                }
            }
            printf("\n");

            // 세로 방향 숫자와 빈 공간 출력
            for (int j = 0; j < WIDTH; j++) {
                if (weights[i][j][1] != INF) {
                    printf(" %d ", weights[i][j][1]);
                }
                else {
                    printf("   "); // 가중치가 INF일 때는 빈 공간
                }

                // 가로 가중치 사이에 빈 공간 추가
                if (j < WIDTH - 1) {
                    printf("       ");
                }
            }
            printf("\n");

            // 세로 방향 막대기 출력 (다음 셀 아래)
            for (int j = 0; j < WIDTH; j++) {
                if (weights[i][j][1] != INF) {
                    printf(" |       ");
                }
                else {
                    printf("         "); // 가중치가 INF일 때는 빈 공간
                }

                if (j < WIDTH - 1) {
                    printf(" ");
                }
            }
            printf("\n");
        }
    }
}

