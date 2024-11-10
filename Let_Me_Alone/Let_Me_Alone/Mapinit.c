#include <stdlib.h>
#include <time.h>
#include "printMap.h"

// ����ġ�� �ʱ�ȭ�ϴ� �Լ�
void initializeWeights(int weights[HEIGHT][WIDTH][4]) {
    srand(time(NULL));

    for (int i = 0; i < HEIGHT; i++) {
        for (int j = 0; j < WIDTH; j++) {
            if (j < WIDTH - 1) {
                // ������ ����ġ ����
                int weight = rand() % 10; // 0���� 9 ������ ������ ����
                weights[i][j][0] = (weight == 0) ? INF : weight;       // ���� ���� ������
                weights[i][j + 1][2] = (weight == 0) ? INF : weight;   // ������ ���� ����
            }
            else {
                weights[i][j][0] = INF; // ���� ������ ��迡�� ����
            }

            if (i < HEIGHT - 1) {
                // �Ʒ��� ����ġ ����
                int weight = rand() % 10; // 0���� 9 ������ ������ ����
                weights[i][j][1] = (weight == 0) ? INF : weight;       // ���� ���� �Ʒ���
                weights[i + 1][j][3] = (weight == 0) ? INF : weight;   // �Ʒ��� ���� ����
            }
            else {
                weights[i][j][1] = INF; // ���� �Ʒ��� ��迡�� ����
            }

            // �� ���ʰ� �� �� ������ ���� ������� �����Ƿ� INF�� ����
            if (j == 0) {
                weights[i][j][2] = INF; // �� ���� ���
            }
            if (i == 0) {
                weights[i][j][3] = INF; // �� �� ���
            }
        }
    }
}


