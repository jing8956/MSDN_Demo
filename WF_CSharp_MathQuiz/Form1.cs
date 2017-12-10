using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WF_CSharp_MathQuiz
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Create a Random object called randomizer  
        // to generate random numbers.
        Random randomizer = new Random();

        // These integer variables store the numbers  
        // for the addition problem.  
        int addend1;
        int addend2;

        // These integer variables store the numbers  
        // for the subtraction problem.  
        int minuend;
        int subtrahend;

        // These integer variables store the numbers  
        // for the multiplication problem.  
        int multiplicand;
        int multiplier;

        // These integer variables store the numbers  
        // for the division problem.  
        int dividend;
        int divisor;

        // This integer variable keeps track of the  
        // remaining time. 
        int timeLeft;

        /// <summary> 
        /// Start the quiz by filling in all of the problems 
        /// and starting the timer. 
        /// </summary> 
        public void StartTheQuiz()
        {
            // Fill in the addition problem. 
            // Generate two random numbers to add. 
            // Store the values in the variables 'addend1' and 'addend2'.
            addend1 = randomizer.Next(51);
            addend2 = randomizer.Next(51);

            // Convert the two randomly generated numbers 
            // into strings so that they can be displayed 
            // in the label controls.
            plusLeftLabel.Text = addend1.ToString();
            plusRightLabel.Text = addend2.ToString();

            // 'sum' is the name of the NumericUpDown control. 
            // This step makes sure its value is zero before 
            // adding any values to it.
            sum.Value = 0;

            // Fill in the subtraction problem.
            minuend = randomizer.Next(1, 101);
            subtrahend = randomizer.Next(1, minuend);
            minusLeftLabel.Text = minuend.ToString();
            minusRightLabel.Text = subtrahend.ToString();
            difference.Value = 0;

            // Fill in the multiplication problem.
            multiplicand = randomizer.Next(2, 11);
            multiplier = randomizer.Next(2, 11);
            timesLeftLabel.Text = multiplicand.ToString();
            timesRightLabel.Text = multiplier.ToString();
            product.Value = 0;

            // Fill in the division problem.
            divisor = randomizer.Next(2, 11);
            int temporaryQuotient = randomizer.Next(2, 11);
            dividend = divisor * temporaryQuotient;
            dividedLeftLabel.Text = dividend.ToString();
            dividedRightLabel.Text = divisor.ToString();
            quotient.Value = 0;

            // Start the timer.
            timeLeft = 30;
            timeLabel.Text = "30 seconds";
            timer1.Start();
        }

        /// <summary> 
        /// Check the answer to see if the user got everything right. 
        /// </summary> 
        /// <returns>True if the answer's correct, false otherwise.</returns> 
        private bool CheckTheAnswer()
        {
            if (addend1 + addend2 == sum.Value
                && (minuend - subtrahend == difference.Value)
                && (multiplicand * multiplier == product.Value)
                && (dividend / divisor == quotient.Value))
                return true;
            else
                return false;
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            StartTheQuiz();
            startButton.Enabled = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (CheckTheAnswer())
            {
                // If CheckTheAnswer() returns true, then the user  
                // got the answer right. Stop the timer   
                // and show a MessageBox.
                timer1.Stop();
                MessageBox.Show("你完成了所有题目！",
                                "祝贺！");
                startButton.Enabled = true;
            }
            else if (timeLeft > 0)
            {
                // Display the new time left 
                // by updating the Time Left label.
                timeLeft = timeLeft - 1;
                timeLabel.Text = timeLeft + " 秒";
            }
            else
            {
                // If the user ran out of time, stop the timer, show 
                // a MessageBox, and fill in the answers.
                timer1.Stop();
                timeLabel.Text = "时间到！";
                MessageBox.Show("你没有在规定的时间内完成。", "对不起！");
                sum.Value = addend1 + addend2;
                difference.Value = minuend - subtrahend;
                product.Value = multiplicand * multiplier;
                quotient.Value = dividend / divisor;
                startButton.Enabled = true;
            }
        }

        private void answer_Enter(object sender, EventArgs e)
        {
            // Select the whole answer in the NumericUpDown control.
            NumericUpDown answerBox = sender as NumericUpDown;

            if (answerBox != null)
            {
                int lengthOfAnswer = answerBox.Value.ToString().Length;
                answerBox.Select(0, lengthOfAnswer);
            }
        }
    }
}
/*自定义布局
 * 当用户只剩下 5 秒时间时，通过设置“timeLabel”控件的“BackColor”属性 (timeLabel.BackColor = Color.Red;) 来使其变为红色。 在完成测验后重置此颜色。 
 * 通过当用户在某个 NumericUpDown 控件中输入正确答案时播放声音来提示用户。（您需要为每个控件的 ValueChanged ValueChanged()事件编写事件处理程序，只要用户更改控件的值，就会触发该事件。） */
