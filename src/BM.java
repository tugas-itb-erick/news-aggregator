class BM {

  public static int bmMatch(String text, String pattern) {
    int[] last = buildLast(pattern);
    int n = text.length();
    int m = pattern.length();
    int i = m-1;

    if (i > n-1){
      return -1;
    }

    int j = m-1;
    do{
      if (pattern.charAt(j) == text.charAt(i)){
        if (j == 0){
          return i;
        }
        else{
          i--;
          j--;
        }
      }
      else{
        int loc = last[text.charAt(i)];
        i += m - Math.min(j, 1+loc);
        j = m - 1;
      }
    }while(i <= n-1);

    return -1;
  }

  public static int[] buildLast(String pattern) {
    final int ASCII_SET = 128;
    int[] last = new int[ASCII_SET];

    for(int i=0; i<ASCII_SET; i++){
      last[i] = -1;
    }

    for(int i=0; i<pattern.length(); i++){
      last[pattern.charAt(i)] = i;
    }

    return last;
  }

  public static void main(String[] args) {
    String text = new String("Competitive Programming Playlist. This playlist gets you from knowing basic programming to being a yellow-red rated coder in CodeChef / Codeforces / TopCoder / etc. Expected time to completion: 40-60 sessions (about 2-3 months). You'll learn about a dozen algorithms while solving problems. Each problem will teach you something new, so make sure you understand it before moving on. The algorithm tutorials include short intuitive video tutorials, as well as links to more in-depth text tutorial. Pre-requisites: Basic knowledge of any programming language. If you don't have the pre-requisite, you should start here: C++ / Java / Python for Competitive Programming Subscribe to get started. Choose which days you would like to get reminder emails (if any). The reminder moves on to the next concept only when you mark current one as completed (self-paced).");
    String pattern = new String("-requisites");

    int pos = bmMatch(text, pattern);
    if (pos == -1)
      System.out.println("Not Found");
    else
      System.out.println("Pattern start at: " + pos);

  }
}
