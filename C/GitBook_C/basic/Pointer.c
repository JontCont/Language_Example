#include <stdlib.h>
#include <stdio.h>

int main()
{
    char s[]="0113256";
    char *p=s;
    printf("%c",*p++);
    printf("%c",*(p++));
    printf("%c",(*p)++);
    printf("%c",*++p);
    printf("%c",*(++p));
    printf("%c",++*p);
    printf("%c",++(*p));

    printf("\n");
    printf(s);
    system("pause");
    return 0;
}