class KMP {

  public static int kmpMatch(String text, String pattern) {
    int[] fail = computeFail(pattern);
    int i = 0, j = 0;

    while(i < text.length()){
      if (pattern.charAt(j) == text.charAt(i)){
        if (j == pattern.length()-1)
          return i - pattern.length() + 1;
        i++;
        j++;
      }
      else if (j > 0){
        j = fail[j-1];
      }
      else{
        i++;
      }
    }

    return -1;
  }

  public static int[] computeFail(String pattern) {
    int[] fail = new int[pattern.length()];
    fail[0] = 0;

    int i = 1;
    int j = 0;
    while(i < pattern.length()){
      if (pattern.charAt(j) == pattern.charAt(i)){
        fail[i++] = ++j;
      }
      else if (j > 0){
        j = fail[j-1];
      }
      else{
        fail[i++] = 0;
      }
    }

    return fail;
  }

  public static void main(String[] args) {
    String text = new String("Competitive Programming Playlist. This playlist gets you from knowing basic programming to being a yellow-red rated coder in CodeChef / Codeforces / TopCoder / etc. Expected time to completion: 40-60 sessions (about 2-3 months). You'll learn about a dozen algorithms while solving problems. Each problem will teach you something new, so make sure you understand it before moving on. The algorithm tutorials include short intuitive video tutorials, as well as links to more in-depth text tutorial. Pre-requisites: Basic knowledge of any programming language. If you don't have the pre-requisite, you should start here: C++ / Java / Python for Competitive Programming Subscribe to get started. Choose which days you would like to get reminder emails (if any). The reminder moves on to the next concept only when you mark current one as completed (self-paced).");
    String pattern = new String("requisites");

    int pos = kmpMatch(text, pattern);
    if (pos == -1)
      System.out.println("Not Found");
    else
      System.out.println("Pattern start at: " + pos);

  }
}
