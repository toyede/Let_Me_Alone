#include <stdio.h>
#include "printMap.h"

void printGrid(int weights[HEIGHT][WIDTH][4]) {
    for (int i = 0; i < HEIGHT; i++) {
        // ���� ���� ���
        printf(" ");
        for (int j = 0; j < WIDTH; j++) {
            printf("��");

            // ������ ����ġ ���
            if (j < WIDTH - 1) {
                if (weights[i][j][0] != INF) {
                    printf("----%d----", weights[i][j][0]);
                }
                else {
                    printf("         "); // ����ġ�� INF�� ���� �� ����
                }
            }
        }
        printf("\n");

        // ���� ���� ����� ���
        if (i < HEIGHT - 1) {
            for (int j = 0; j < WIDTH; j++) {
                if (weights[i][j][1] != INF) {
                    printf(" |       ");
                }
                else {
                    printf("         "); // ����ġ�� INF�� ���� �� ����
                }

                if (j < WIDTH - 1) {
                    printf(" ");
                }
            }
            printf("\n");

            // ���� ���� ���ڿ� �� ���� ���
            for (int j = 0; j < WIDTH; j++) {
                if (weights[i][j][1] != INF) {
                    printf(" %d ", weights[i][j][1]);
                }
                else {
                    printf("   "); // ����ġ�� INF�� ���� �� ����
                }

                // ���� ����ġ ���̿� �� ���� �߰�
                if (j < WIDTH - 1) {
                    printf("       ");
                }
            }
            printf("\n");

            // ���� ���� ����� ��� (���� �� �Ʒ�)
            for (int j = 0; j < WIDTH; j++) {
                if (weights[i][j][1] != INF) {
                    printf(" |       ");
                }
                else {
                    printf("         "); // ����ġ�� INF�� ���� �� ����
                }

                if (j < WIDTH - 1) {
                    printf(" ");
                }
            }
            printf("\n");
        }
    }
}

