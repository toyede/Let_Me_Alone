#pragma once

#define WIDTH 11
#define HEIGHT 7
#define INF -1

// 가중치를 가진 격자 출력 함수 선언
void printGrid(int weights[HEIGHT][WIDTH][4]);

// 가중치 초기화 함수 선언
void initializeWeights(int weights[HEIGHT][WIDTH][4]);
