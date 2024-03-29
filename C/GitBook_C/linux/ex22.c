* 實習22：I-7000模組綜合練習
【實習目的】
寫一個可同時監控I-7060、I-7012、及I-7021等分散式模組的Linux C程式。

【動作需求】
以5V電壓經由一可變電阻改變電壓大小，當作I-7012模組的輸入類比電壓訊號，I-7021模組輸出端則接著一個直流電壓表，此練習希望撰寫一個Linux C程式，達到下列的動作要求：
(1) 讀取I-7012模組的輸入電壓，將讀到的值輸出至I-7021模組
(2) 若輸入電壓為【0V – 1.5V】，I-7060的Relay 1 ON，其餘OFF。
(3) 若輸入電壓為【1.5V- 3V】，I-7060的Relay 2 ON，其餘OFF。
(4) 若輸入電壓為【3V - 3.5V】，I-7060的Relay3 ON，其餘OFF。
(5) 若輸入電壓為【3.5V - 5V】，I-7060的Relay 4 ON，其餘OFF。
(6) 若輸入電壓大於5V，I-7060的四個Relay反覆ON、OFF，其時間間隔為1秒。
(7) 若輸入電壓為零，則離開程式結束動作。

【程式】
/* ex7000_all.c */
 #include "i7k.h"
 #include "stdio.h"
 #include "time.h"
 #include "stdlib.h"
 #include <sys/types.h>
 #include <sys/stat.h>
 #include <fcntl.h>
 #include <termios.h>

 #define BAUDRATE B9600
 #define MODEMDEVICE "/dev/ttyS0"
 #define FALSE 0
 #define TRUE 1
 #define RCVBUF_SIZE 16

 int main()
 {
  int 		fd,i,r;
  float 		num;
  char 		*cmd,cmd2[100],*cmd3;
  char 		rbuf[RCVBUF_SIZE],rbuf1[RCVBUF_SIZE];
  __tod_t 	texp;
  double 	dout;

  fd=sio_open("/dev/ttyS0",B9600,DATA_BITS_8,NO_PARITY,ONE_STOP_BIT);
  if (fd==ERR_PORT_OPEN)
   {
    printf("open port failed!\n");
    return(-1);
   }
  cmd3="$016\r"; 
  while (num!=0)
  {	  
   sio_set_noncan(fd);
   sio_set_timer(fd,0,10);
   cmd="#02\r";
   r=i7k_send_readt(fd,cmd,rbuf,RCVBUF_SIZE,&texp);
   rbuf[r]=0;
   printf("send:%s\n",rbuf);

   for (i=0;i<6;i++) {
     rbuf1[i]=rbuf[i+3];
   }
   rbuf1[6]='\r';
   strcpy(cmd2,"#030");
   strcat(cmd2,rbuf1);
   r=i7k_send_readt(fd,cmd2,rbuf,RCVBUF_SIZE,&texp);

   num=atof(rbuf1);
  I f (num==0) {
      cmd3 = "#010000\r";
      i7k_send_readt(fd, cmd3, rbuf, RCVBUF_SIZE, &texp);
   }
   else if (num > 0.0 && num <= 1.5) {
      cmd3 = "#010001\r";
      i7k_send_readt(fd, cmd3, rbuf, RCVBUF_SIZE, &texp);
   }
   else if (num > 1.5 && num <= 3.0) {
      cmd3 = "#010003\r";
      i7k_send_readt(fd, cmd3, rbuf, RCVBUF_SIZE, &texp);
   }
   else if (num > 3.0 && num <= 3.5) {
      cmd3 = "#010007\r";
      i7k_send_readt(fd, cmd3, rbuf, RCVBUF_SIZE, &texp);
   }
   else if (num > 3.5 && num <=5.0) {
      cmd3 = "#01000F\r";
      i7k_send_readt(fd, cmd3, rbuf, RCVBUF_SIZE, &texp);
   }
   else if (num > 5.0)  {
      cmd3 = "#01000F\r";
      i7k_send_readt(fd, cmd3, rbuf, RCVBUF_SIZE, &texp);
      sleep(1);
      cmd3 = "#010000\r";
      i7k_send_readt(fd, cmd3, rbuf, RCVBUF_SIZE, &texp);
      sleep(1);
   } /* if */
  } /* while */
  sio_close(fd);
  return(0);
 }


