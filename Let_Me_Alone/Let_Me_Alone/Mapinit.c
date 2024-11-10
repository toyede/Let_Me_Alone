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
                weights[i][j][0] = weight;        // ���� ���� ������
                weights[i][j + 1][2] = weight;    // ������ ���� ����
            }
            else {
                weights[i][j][0] = 0; // ���� ������ ��迡�� ����
            }

            if (i < HEIGHT - 1) {
                // �Ʒ��� ����ġ ����
                int weight = rand() % 10; // 0���� 9 ������ ������ ����
                weights[i][j][1] = weight;        // ���� ���� �Ʒ���
                weights[i + 1][j][3] = weight;    // �Ʒ��� ���� ����
            }
            else {
                weights[i][j][1] = 0; // ���� �Ʒ��� ��迡�� ����
            }

            // �� ���ʰ� �� �� ������ ���� ������� �����Ƿ� 0���� ����
            if (j == 0) {
                weights[i][j][2] = 0; // �� ���� ���
            }
            if (i == 0) {
                weights[i][j][3] = 0; // �� �� ���
            }
        }
    }
}

