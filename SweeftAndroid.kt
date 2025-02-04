package com.example.swiftexercises


/*1. გვაქვს მთელი რიცხვების ჩამონათვალი სადაც  ერთის გარდა ყველა რიცხვი  მეორდება, იპოვეთ ის რიცხვი
რომელიც არ მეორდება.int singleNumber(int[] nums)*/
fun findUniqueNumber(numbers: IntArray): Int {
    if (numbers.isEmpty()) throw IllegalArgumentException("array can't be empty")
    return numbers.fold(0) { result, number ->
        result xor number
    }
}

/*2. გვაქვს 1,5,10,20 და 50 თეთრიანი მონეტები. დაწერეთ ფუნქცია, რომელსაც გადაეცემა თანხა (თეთრებში)
და აბრუნებს მონეტების მინიმალურ რაოდენობას, რომლითაც შეგვიძლია ეს თანხა დავახურდაოთ.
Int minSplit(Int amount); */
fun calculateMinCoins(amount: Int): Int {
    require(amount >= 0) { "amount must be negative" }

    var remainingAmount = amount
    val coinValues = listOf(50, 20, 10, 5, 1)

    return coinValues.sumOf { coin ->
        val count = remainingAmount / coin
        remainingAmount %= coin
        count
    }
}

/*3. მოცემულია მასივი, რომელიც შედგება სიტყვებისგან(string). დაწერეთ ფუნქცია რომელსაც
გადაეცემა ეს მასივი და აბრუნებს ყველაზე გრძელ თავსართს(prefix) რომელიც მეორდება ამ
სიტყვებში. თუ არცერთი არ მეორდება აბრუნებს ცარიელ სტრინგს.მაგ:[“extract”,”exhale”,
“excavate”] , პასუხი იქნება “ex”
String longestPrefix(String[] array);*/
fun findLongestCommonPrefix(words: Array<String>): String {
    if (words.isEmpty()) return ""
    val firstWord = words[0]

    for (charPosition in firstWord.indices) {
        val currentChar = firstWord[charPosition]
        for (word in words) {
            if (charPosition >= word.length || word[charPosition] != currentChar) {
                return firstWord.substring(0, charPosition)
            }
        }
    }
    return firstWord
}

/*4.მოცემულია ორი binary string a და b, დააბრუნეთ მათი ჯამი, როგორც binary string.
მაგ: a = "1010" b = "1011" , მათი ჯამი იქნება "10101"*/
fun addBinaryNumbers(first: String, second: String): String {
    val result = StringBuilder()
    var firstPointer = first.lastIndex
    var secondPointer = second.lastIndex
    var carry = 0

    while (firstPointer >= 0 || secondPointer >= 0 || carry > 0) {
        val firstDigit = if (firstPointer >= 0) first[firstPointer--] - '0' else 0
        val secondDigit = if (secondPointer >= 0) second[secondPointer--] - '0' else 0

        val total = firstDigit + secondDigit + carry
        result.append(total % 2)
        carry = total / 2
    }

    return result.reverse().toString()
}

/*5. გვაქვს n სართულიანი კიბე, ერთ მოქმედებაში შეგვიძლია ავიდეთ 1 ან 2 საფეხურით. დაწერეთ ფუნქცია
რომელიც დაითვლის n სართულზე ასვლის ვარიანტების რაოდენობას.
Int countVariants(Int stearsCount);*/
fun countStairClimbingWays(steps: Int): Int {
    require(steps >= 0) { "step number can't be negative" }
    if (steps <= 1) return 1

    var prev = 1
    var current = 1
    repeat(steps - 1) {
        val next = prev + current
        prev = current
        current = next
    }
    return current
}

/*6. დაწერეთ საკუთარი მონაცემთა სტრუქტურა, რომელიც საშუალებას მოგვცემს O(1) დროში წავშალოთ
ელემენტი.*/
class FastRemovalCollection {
    private val elements = mutableListOf<Int>()
    private val indexMap = mutableMapOf<Int, Int>()

    fun add(value: Int) {
        if (value in indexMap) return
        indexMap[value] = elements.size
        elements.add(value)
    }

    fun remove(value: Int) {
        indexMap[value]?.let { position ->

            val lastElement = elements.last()
            elements[position] = lastElement
            indexMap[lastElement] = position


            elements.removeAt(elements.lastIndex)
            indexMap.remove(value)
        }
    }

    fun contains(value: Int): Boolean = indexMap.contains(value)
}

fun main() {
    // 1.
    println("unique number: ${findUniqueNumber(intArrayOf(4, 1, 2, 1, 2))}") // 4

    // 2.
    println("minimum coins for 87: ${calculateMinCoins(87)}") // 6

    // 3.
    println("longest prefix: ${findLongestCommonPrefix(arrayOf("flower", "flow", "flight"))}") // "fl"

    // 4.
    println("binary sum: ${addBinaryNumbers("1010", "1011")}")

    // 5.
    println("ways to climb 4 steps: ${countStairClimbingWays(4)}")

    // 6.
    val numberBag = FastRemovalCollection().apply {
        add(1)
        add(2)
        add(3)
        remove(2)
    }
    println("contains 2? ${numberBag.contains(2)}")
}