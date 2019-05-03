import java.awt.Toolkit;
import java.awt.event.KeyEvent;
import java.util.regex.Matcher;  
import java.util.regex.Pattern; 
import java.lang.*;
import java.io.*;

PFont font;
String textCode = "";
int fontSize = 24;
int indentWord = 10;
color tegColor = color(250, 123, 170);
color mainColor = color(250, 250, 250);
int curStartXWord = 1;
int curStartYWord = 1;
int caretkaX = 1;
int caretkaY = 1;
ArrayList<Integer> lineLengths = new ArrayList<Integer>();
boolean isActiveCaretka = false;
int indexInsertChar = 0;
String debagLeftPart = "";
String debagRightPart = "";

void setup() {
  size(1240, 980);
  //println(PFont.list());
  background(30, 30, 30, 0.8);
  font = createFont("Candara", fontSize);
  textFont(font);
  textSize(fontSize);
}

void draw() {
  fill(50, 20, 50);
  rect(0,0,width,height);
  drawText();
  drawCaretka();
  
  //Debug data
  text(caretkaX + ":" + caretkaY, width-150,0+50);
  text("curStartXWord= " + textCode.length(), width-150,0+100);
  text("indexInsetX= " + indexInsertChar, width-150,0+130);
  text("ca= " + debagLeftPart, width-150, 0+160);
  text("RightPart= " + debagLeftPart, width-150, 0+190);
  text("TextCode= " + textCode, width-150, 0+220);
}

void drawText() {
  Pattern patternTag = Pattern.compile("</?\\w*>");
  Matcher tags = patternTag.matcher(textCode);
  
  for(int i = 0, j = 1, y = 1; i < textCode.length(); i++,j++) {
    textAlign(CENTER, BASELINE);
    if(textCode.charAt(i)=='\n') {y += 1;j=0;}
    fill(255);
    while(tags.find()) {
      if(i > tags.start() && i < tags.end()-1) {
        fill(250, 20, 20);
      }
    }
    tags.reset();
    text(textCode.charAt(i), j*(fontSize-6), y*fontSize);
  }
}

void drawCaretka() {
  fill(120, 250, 120);
  rect((caretkaX-1)*(fontSize-6), (caretkaY-1)*fontSize, fontSize/4, fontSize);
}

void keyPressed() {
   if(lineLengths.size() != 0) {
    lineLengths = new ArrayList<Integer>();
    String repliceTextCode = new String(String.valueOf(textCode));
    int count = 0;
    
    for(int i = 0; i < textCode.length(); i++) {    
      if(textCode.charAt(i) == '\n') { 
        String curLine = repliceTextCode.substring(0, repliceTextCode.indexOf('\n')+1);
        repliceTextCode = repliceTextCode.substring(repliceTextCode.indexOf('\n')+1, repliceTextCode.length());
        
        lineLengths.add(curLine.length());
      }
    }
  }
  
  if(keyCode == TAB) {
    String[] res = getIndexInsertChar();
    textCode = res[0] + '\t' + res[1];
    caretkaX++;
    curStartXWord++;
  }
  
  if(keyCode == ENTER) {
      
      String[] res = getIndexInsertChar();
      textCode = res[0] + '\n' + res[1];
    
      caretkaY++;
      curStartYWord++;
      curStartXWord++;
      caretkaX = 1;
      
      Pattern p = Pattern.compile("\\n");  
      Matcher matches = p.matcher(textCode);
      
      lineLengths = new ArrayList<Integer>();
      int countStart = 0;
      
      while(matches.find()) {
         //println(textCode.substring(countStart,matches.end()));
         int lengthLine = textCode.substring(countStart,matches.end()).length();
         lineLengths.add(lengthLine);
         countStart += lengthLine;
      }
  }
  if(keyCode == BACKSPACE) {
    
    if(textCode.length() != 0) {
        
        String[] res = getIndexInsertChar();
        textCode = res[0].substring(0, res[0].length()-1) + res[1];
      
        if(caretkaX == 1) {
          caretkaX = lineLengths.get(caretkaY-2);
          lineLengths.remove(caretkaY-2);
          caretkaY--;
          curStartYWord--;
        } else {
          caretkaX--;
        }
        
        curStartXWord--;
    }
  } 
  
  switch(keyCode) {
    case 37:
      if(caretkaY != 1) {
        if(caretkaX > 1) {
          caretkaX--;
        } else {
          int s = lineLengths.size() != 0 ? lineLengths.get(caretkaY-2) : textCode.length();
          caretkaX = s;
          caretkaY--;
        }
      } else {
        caretkaX = (caretkaX > 1) ? --caretkaX : 1;
      }
      break;
    case 38:
      if(lineLengths.size() != 0) {
        if(caretkaY != 1 && caretkaX > lineLengths.get(caretkaY-2)) {
          caretkaX = lineLengths.get(caretkaY-2);
        }
      }
      caretkaY -= (caretkaY > 1) ? 1 : 0;
      break;
    case 39:
      if(caretkaY != curStartYWord) {
        int s = lineLengths.size() != 0 ? lineLengths.get(caretkaY-1) : textCode.length();
        if(caretkaX < s) {
           caretkaX++;
        } else {
          caretkaX = 1;
          caretkaY++;
        }
      } else {
        int lastLineLength;
        if(textCode.lastIndexOf('\n') != -1) {
          lastLineLength = textCode.length() - (textCode.substring(0, textCode.lastIndexOf('\n')).length());
        } else {
          lastLineLength = textCode.length()+1;
        }
        if(caretkaX < lastLineLength) {
           caretkaX++;
        }
      }
      break;
    case 40:
      if(caretkaY < curStartYWord) {
        int lengthSummaLines = 0;
        for(int i = 0; i < caretkaY; i++) {lengthSummaLines += lineLengths.get(i);}
        int line = caretkaY != lineLengths.size() ? lineLengths.get(caretkaY) : textCode.length() - lengthSummaLines;
        if(lineLengths.size() != 0 && (caretkaX > line)) {
          String downPart = textCode.substring(lengthSummaLines, textCode.length());
          println(downPart);
          int startSliceIndex = downPart.indexOf('\n') != -1 ?  downPart.indexOf('\n') : downPart.length();
          caretkaX = (downPart.length() - downPart.substring(startSliceIndex, downPart.length()).length())+1;
        }
        caretkaY++;
      }
      break;
      
 }
  
  if(((keyCode <= 15 || keyCode >= 21) && keyCode != 8 && keyCode != 10 && !(keyCode >= 37 && keyCode <= 40)) || (keyCode >= 65 && keyCode <= 90)) {
    try {
      String[] res = getIndexInsertChar();
      textCode = res[0] + key + res[1];
      
      curStartXWord += 1; 
      caretkaX++;
       
    } catch(StringIndexOutOfBoundsException exp) {
      println("ERROR!!!");
    }
  }
}

String[] getIndexInsertChar() {
  String[] result = new String[2];
  String leftPart;
  String rightPart;
  indexInsertChar = 0;
  
  for(int i = 0; i < caretkaY-1; i++) { indexInsertChar += lineLengths.get(i); }
  
  indexInsertChar += caretkaX;
  
  if(textCode.length() != 0) {
    if(indexInsertChar != curStartXWord) {
      leftPart = textCode.substring(0, indexInsertChar-1);
      rightPart = textCode.substring(indexInsertChar-1, textCode.length());
    } else {
      leftPart = textCode;
      rightPart = "";
    }
  } else {
    leftPart = "";
    rightPart = "";
  }
  
  debagLeftPart = leftPart;
  debagRightPart = rightPart;
  
  result[0] = leftPart;
  result[1] = rightPart;
  
  return result;
}
