#include "printMap.h"
#include <stdio.h>
#include <stdlib.h>
#include <time.h>

int main() {

    // 각 셀의 가중치 정의: [오른쪽, 아래, 왼쪽, 위]
    int weights[HEIGHT][WIDTH][4];

    // 가중치 초기화
    initializeWeights(weights);

    // 가중치를 가진 격자 출력
    printGrid(weights);

    return 0;
}

